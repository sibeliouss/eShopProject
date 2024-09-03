using Domain.Abstract;

namespace Domain.Entities;

public class Address : Entity<Guid>
{
    public Guid CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public string Country { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string? ZipCode { get; set; }
    public string ContactName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty; 
}