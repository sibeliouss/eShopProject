namespace Application.Features.Customers.Dtos;

public record RegisterDto(
    string FirstName,
    string LastName,
    string Email,
    string UserName,
    string Password,
    string ConfirmedPassword);