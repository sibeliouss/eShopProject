namespace Application.Features.BillingAddresses.Dtos;

public record UpdateBillingAddressDto(
    Guid Id,
    Guid CustomerId,
    string Country,
    string City,
    string ZipCode,
    string ContactName,
    string Description);