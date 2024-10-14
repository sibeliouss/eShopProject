namespace Application.Features.Customers.Dtos;

public class CustomerDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Email { get; set; }
    public string? UserName { get; set; }
    
}