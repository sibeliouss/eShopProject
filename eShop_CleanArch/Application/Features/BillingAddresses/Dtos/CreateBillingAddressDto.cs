namespace Application.Features.BillingAddresses.Dtos;

public record CreateBillingAddressDto(
    Guid CustomerId,
    string Country,
    string City,
    string? ZipCode,
    string ContactName,
    string Description);