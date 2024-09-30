namespace Application.Features.ProductDiscounts.Dtos;

public record ProductDiscountDto
(
    Guid ProductId,
    int DiscountPercentage,
    DateTime StartDate,
    DateTime EndDate
);