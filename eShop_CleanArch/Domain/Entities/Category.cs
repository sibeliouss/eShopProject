using Domain.Abstract;

namespace Domain.Entities;

public class Category : Entity<Guid>
{
    public string Name { get; set; }
    public string IconImgUrl { get; set; } 
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; } = false;
    public ICollection<ProductCategory>? ProductCategories { get; set; }


}