using Domain.Abstract;

namespace Domain.Entities;

public class BillingAddress : Entity<Guid>
{
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public string Country { get; set; } 
    public string City { get; set; } 
    public string? ZipCode { get; set; }
    public string ContactName { get; set; } 
    public string Description { get; set; } 
}