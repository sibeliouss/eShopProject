namespace Application.Features.ProductDiscounts.Dtos;

public record UpdateProductDiscountDto(
    Guid Id,
    int DiscountPercentage,
    DateTime StartDate,
    DateTime EndDate);