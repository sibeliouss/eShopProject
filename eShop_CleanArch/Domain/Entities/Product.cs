using Domain.Abstract;

namespace Domain.Entities;

public class Product : Entity<Guid>
{
    public string Name { get; set; }
    public string Brand { get; set; }
    public string Img { get; set; }
    public string? Description { get; set; }
    public int Stock { get; set; }
    public string Barcode { get; set; }
    public bool IsActive { get; set; } = true;
    public Guid CategoryId { get; set; }
    public Category? Category { get; set; }
    public bool IsFeatured { get; set; } = false;
    public bool IsDeleted { get; set; } = false;
    public ICollection<ProductCategory>? ProductCategories { get; set; }
}