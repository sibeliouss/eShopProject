namespace Application.Features.BillingAddresses.Dtos;

public record UpdateBillingAddressDto(
    Guid Id,
    Guid UserId,
    string Country,
    string City,
    string ZipCode,
    string ContactName,
    string Description);