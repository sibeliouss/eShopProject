using Domain.Abstract;
using Domain.Entities.ValueObjects;

namespace Domain.Entities;

public class OrderDetail : Entity<Guid>
{
    public Guid OrderId { get; set; }
    public Order? Order { get; set; }
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    public int Quantity { get; set; }
    public Money Price { get; set; } = new(0, "₺");
    
}