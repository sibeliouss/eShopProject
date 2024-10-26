using Domain.Entities;
using Domain.Entities.ValueObjects;

namespace Application.Features.WishLists.Queries.Responses;

public class WishListResponse
{
    public Guid Id { get; set; }
    public Guid DetailId { get; set; }
    public ProductDetail? ProductDetail { get; set; }
    public string Name { get; set; } 
    public string Brand { get; set; }  
    public Money Price { get; set; } = new(0, "₺");
    public string Img { get; set; } 
    public bool IsFeatured { get; set; } 
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } 
    public DateTime CreateAt { get; set; } = DateTime.Now;
    public List<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();

    public Guid WishListId { get; set; }
}