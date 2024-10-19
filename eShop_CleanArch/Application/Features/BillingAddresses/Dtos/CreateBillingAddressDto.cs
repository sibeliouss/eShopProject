namespace Application.Features.BillingAddresses.Dtos;

public record CreateBillingAddressDto(
    Guid UserId,
    string Country,
    string City,
    string? ZipCode,
    string ContactName,
    string Description);