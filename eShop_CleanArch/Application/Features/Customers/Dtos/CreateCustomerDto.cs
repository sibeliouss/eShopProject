using System.Net.WebSockets;

namespace Application.Features.Customers.Dtos;

public record CreateCustomerDto(
    string FirstName,
    string LastName, 
    string Email, 
    Guid UserId,
    string UserName, 
    string PasswordHash
);