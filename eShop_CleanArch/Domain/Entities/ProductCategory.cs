using Domain.Abstract;

namespace Domain.Entities;

public class ProductCategory : Entity<Guid>
{
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    
    public Guid CategoryId { get; set; }
    public Category? Category { get; set; }
}