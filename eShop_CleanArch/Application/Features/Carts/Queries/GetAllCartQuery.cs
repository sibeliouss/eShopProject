using Application.Features.Carts.Queries.Responses;
using Application.Services.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Carts.Queries;

public class GetAllCartQuery : IRequest<List<CartResponse>>
{
  public Guid UserId { get; set; }
  public class GetAllCartQueryHandler: IRequestHandler<GetAllCartQuery,List<CartResponse>>
  {
    private readonly ICartRepository _cartRepository;

    public GetAllCartQueryHandler(ICartRepository cartRepository)
    {
      _cartRepository = cartRepository;
    }
    public async Task<List<CartResponse>> Handle(GetAllCartQuery request, CancellationToken cancellationToken)
    {
      var cartResponse = await _cartRepository.Query().Where(b => b.UserId == request.UserId).AsNoTracking()
        .Include(b => b.Product).Select(b => new CartResponse()
        {
          CartId = b.Id,
          CreateAt = b.CreateAt,
          Id = b.Product.Id,
          Img = b.Product.Img,
          IsActive = b.Product.IsActive,
          IsDeleted = b.Product.IsDeleted,
          IsFeatured = b.Product.IsFeatured,
          Price = b.Price,
          Quantity = b.Quantity,
          ProductDetail = b.Product.ProductDetail,
          Name = b.Product.Name,
          Brand = b.Product.Brand,
          ProductCategories = b.Product.ProductCategories
        }).ToListAsync(cancellationToken);

      return cartResponse;
    }
  }
}