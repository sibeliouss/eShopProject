using Application.Features.Products.Dtos;
using Domain.Entities.ValueObjects;

namespace Application.Features.Products.Queries.ResponseDtos;

public class GetProductResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Brand { get; set; }
    public string Img { get; set; }
    public int Quantity { get; set; }
    public Money Price { get; set; } = new(0, "TRY");
    public ProductDetailDto? ProductDetail { get; set; }
    public ProductDiscountDto? ProductDiscount { get; set; }
    public bool IsFeatured { get; set; }
    public DateTime CreateAt { get; set; } = DateTime.Now;
   

}