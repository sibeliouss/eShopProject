
using Application.Features.Products.Dtos;
using Domain.Entities.ValueObjects;

namespace Application.Features.Products.Queries.ResponseDtos;

public class ProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Brand { get; set; }
    public string Img { get; set; }
    
    public Money Price { get; set; } = new(0, "TRY");
    
    public int Quantity { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsFeatured { get; set; } = false;
    public bool IsDeleted { get; set; } = false;
    public DateTime CreateAt { get; set; } = DateTime.Now;
    public ICollection<ProductCategoryDto> ProductCategories { get; set; } = new List<ProductCategoryDto>();

}