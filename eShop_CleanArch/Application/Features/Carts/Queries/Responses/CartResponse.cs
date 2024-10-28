using Domain.Entities;
using Domain.Entities.ValueObjects;

namespace Application.Features.Baskets.Queries.Responses;

public class CartResponse
{
    public Guid Id { get; set; }
    public Guid ProductDetailId { get; set; }
    public ProductDetail? ProductDetail { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public Money Price { get; set; } = new(0, "₺");
    public string Img { get; set; } = string.Empty;
    public int Quantity { get; set; } = 0;
    public bool IsFeatured { get; set; } = false;
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
    public DateTime CreateAt { get; set; } = DateTime.Now;
    public ICollection<ProductCategory>? ProductCategories { get; set; }
    public Guid CartId { get; set; }
}