using Domain.Abstract;
using Domain.Entities.ValueObjects;

namespace Domain.Entities;

public class WishList : Entity<Guid>
{
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
    
    public Money Price { get; set; } = new(0, "TRY");
}