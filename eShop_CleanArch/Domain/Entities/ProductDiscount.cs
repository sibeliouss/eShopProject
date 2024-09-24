using Domain.Abstract;

namespace Domain.Entities;

public class ProductDiscount : Entity<Guid>
{
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    public int DiscountPercentage { get; set; } = 0;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal DiscountedPrice { get; set; } = 0;
    
}