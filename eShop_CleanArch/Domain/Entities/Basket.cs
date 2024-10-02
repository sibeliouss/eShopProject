using Domain.Abstract;
using Domain.Entities.ValueObjects;

namespace Domain.Entities;

public class Basket : Entity<Guid>
{
    public Guid CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    public int Quantity { get; set; }
    public Money Price { get; set; } = new(0, "₺");
}
