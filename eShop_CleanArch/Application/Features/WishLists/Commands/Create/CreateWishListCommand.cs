using Application.Services.Repositories;
using Domain.Entities;
using Domain.Entities.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.WishLists.Commands.Create;

public class CreateWishListCommand : IRequest
{
   public Guid ProductId { get; set; }
   public Guid UserId { get; set; }
   public Money Price { get; set; }

   public class CreateWishListCommandHandler : IRequestHandler<CreateWishListCommand>
   {
      private readonly IWishListRepository _wishListRepository;

      public CreateWishListCommandHandler(IWishListRepository wishListRepository)
      {
         _wishListRepository = wishListRepository;
      }

      public async Task Handle(CreateWishListCommand request, CancellationToken cancellationToken)
      {
         var wishList = await _wishListRepository.Query()
            .Where(w => w.ProductId == request.ProductId && w.UserId == request.UserId)
            .FirstOrDefaultAsync(cancellationToken);

         if (wishList is not null)
         {
            throw new Exception("bu ürün zaten favorilerde ekli");
         }
         
         var newWishList = new WishList
         {
            ProductId  = request.ProductId,
            UserId = request.UserId,
            Price = request.Price
         };

         await _wishListRepository.AddAsync(newWishList);
      }
   }
}