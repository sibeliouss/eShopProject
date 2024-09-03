using Domain.Abstract;

namespace Domain.Entities;

public class Category : Entity<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string IconImgUrl { get; set; } = string.Empty;
    public bool IsActive { get; set; } = false;
    public bool IsDeleted { get; set; } = false;
    public ICollection<ProductCategory>? ProductCategories { get; set; }
}