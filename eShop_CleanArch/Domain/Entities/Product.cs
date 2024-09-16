using Domain.Abstract;
using Domain.Entities.ValueObjects;

namespace Domain.Entities;

public class Product : Entity<Guid>
{
    public string Name { get; set; }
    public string Brand { get; set; }
    public string Img { get; set; }
    public string? Description { get; set; }
    public Money Price { get; set; } = new(0, "₺");
    public int Stock { get; set; }
    public string Barcode { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsFeatured { get; set; } = false;
    public bool IsDeleted { get; set; } = false;
    public ICollection<ProductCategory>? ProductCategories { get; set; }
}