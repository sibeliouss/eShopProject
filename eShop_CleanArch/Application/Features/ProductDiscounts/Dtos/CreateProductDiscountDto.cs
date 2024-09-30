namespace Application.Features.ProductDiscounts.Dtos;

public record CreateProductDiscountDto
(
    Guid ProductId,
    int DiscountPercentage,
    DateTime StartDate,
    DateTime EndDate
);