using Domain.Entities.ValueObjects;

namespace Application.Features.Products.Dtos;

public class ProductDto
{
    public Guid Id { get; set; }
    public bool IsFeatured { get; set; } = false;
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
    public string Name { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Img { get; set; }= string.Empty;
    public ProductDetailDto ProductDetail { get; set; }
    public Money Price { get; set; } = new Money(0, "₺");
  
    public DateTime CreateAt { get; set; } = DateTime.Now;
    public List<ProductCategoryDto> ProductCategories { get; set; } = new List<ProductCategoryDto>();

}