namespace Application.Features.Customers.Commands.Create;

public class CreatedCustomerResponse
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Email { get; set; }
    public string? UserName { get; set; }
    public Guid UserId { get; set; }
   
}