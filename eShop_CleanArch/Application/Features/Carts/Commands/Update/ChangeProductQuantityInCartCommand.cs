using Application.Services.Products;
using Application.Services.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Carts.Commands.Update;

public class ChangeProductQuantityInCartCommand : IRequest
{
   public Guid ProductId { get; set; } 
   public int Quantity { get; set; }
   
   public class  GetChangeProductQuantityInCartQueryHandler : IRequestHandler<ChangeProductQuantityInCartCommand>
   {
      private readonly ICartRepository _cartRepository;
      private readonly IProductService _productService;

      public GetChangeProductQuantityInCartQueryHandler(ICartRepository cartRepository, IProductService productService)
      {
         _cartRepository = cartRepository;
         _productService = productService;
      }
      public async Task Handle(ChangeProductQuantityInCartCommand request, CancellationToken cancellationToken)
      {
         var cart = await _cartRepository.Query().Where(b => b.ProductId == request.ProductId)
            .FirstOrDefaultAsync(cancellationToken);
         if (cart is null) throw new Exception("Ürün bulunamadı!");
         if (request.Quantity == 0)
         {
            await _cartRepository.DeleteAsync(cart);
         }
         else
         {
            var product = await _productService.GetByIdAsync(request.ProductId);
            if(product is not null)
            {
               if(product.Quantity < request.Quantity)
               {
                  throw new Exception("Ürün stoğu yeterli değil.");
               }
               else
               {
                  cart.Quantity = request.Quantity;
                  await _cartRepository.UpdateAsync(cart);
               }
            }
            
         }
      }
   }
}