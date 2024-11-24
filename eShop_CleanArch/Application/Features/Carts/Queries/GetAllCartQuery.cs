using Application.Features.Carts.Queries.Responses;
using Application.Features.Products.Dtos;
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
      var currentDate = DateTime.Now; 
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
          ProductDetail = b.Product.ProductDetail != null
            ? new ProductDetailDto
            {
              Id = b.Product.ProductDetail.Id,
              ProductId = b.Product.Id,
              Barcode = b.Product.ProductDetail.Barcode,
              Description = b.Product.ProductDetail.Description,
              Material = b.Product.ProductDetail.Material,
              Fit = b.Product.ProductDetail.Fit,
              Size = b.Product.ProductDetail.Size,
              Color = b.Product.ProductDetail.Color
            }
            : null,
          ProductDiscount = b.Product.ProductDiscount != null &&
                            b.Product.ProductDiscount.StartDate <= currentDate && 
                            b.Product.ProductDiscount.EndDate >= currentDate   
            ? new ProductDiscountDto()
            {
              Id = b.Product.ProductDiscount.Id,
              ProductId = b.Product.Id,
              DiscountedPrice = b.Product.ProductDiscount.DiscountedPrice,
              DiscountPercentage = b.Product.ProductDiscount.DiscountPercentage,
              StartDate = b.Product.ProductDiscount.StartDate,
              EndDate = b.Product.ProductDiscount.EndDate
            }
            : null,
          Name = b.Product.Name,
          Brand = b.Product.Brand,
          ProductCategories = b.Product.ProductCategories
        }).ToListAsync(cancellationToken);

      return cartResponse;
    }
  }
}