namespace Application.Features.ProductDiscounts.Dtos;

public record ProductDiscountDto
(
    Guid Id,
    Guid ProductId,
    int DiscountPercentage,
    DateTime StartDate,
    DateTime EndDate,
    decimal DiscountedPrice
);