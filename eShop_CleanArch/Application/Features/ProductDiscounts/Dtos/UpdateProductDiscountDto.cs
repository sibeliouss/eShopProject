namespace Application.Features.ProductDiscounts.Dtos;

public record UpdateProductDiscountDto(
    Guid Id,
    Guid ProductId,
    int DiscountPercentage,
    DateTime StartDate,
    DateTime EndDate);