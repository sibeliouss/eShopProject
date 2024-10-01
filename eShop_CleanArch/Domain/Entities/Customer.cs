using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class Customer : IdentityUser<Guid>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public User? User { get; set; }
    
    public ICollection<Address> Addresses { get; set; } = new List<Address>();
    public ICollection<Order> Orders { get; set; } = new List<Order>();

}