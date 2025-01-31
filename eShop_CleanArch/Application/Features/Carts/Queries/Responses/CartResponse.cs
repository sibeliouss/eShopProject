using Application.Features.Products.Dtos;
using Domain.Entities;
using Domain.Entities.ValueObjects;

namespace Application.Features.Carts.Queries.Responses;

public class CartResponse
{
    public Guid Id { get; set; }
    public ProductDetailDto? ProductDetail { get; set; }
    public ProductDiscountDto? ProductDiscount { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public Money Price { get; set; } = new(0, "TRY");
    public string Img { get; set; } = string.Empty;
    public int Quantity { get; set; } = 0;
    public bool IsFeatured { get; set; } = false;
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
    public DateTime CreateAt { get; set; } = DateTime.Now;
    public ICollection<ProductCategory>? ProductCategories { get; set; }
    public Guid CartId { get; set; }
}