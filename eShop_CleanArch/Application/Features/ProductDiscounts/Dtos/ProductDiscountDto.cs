using Domain.Entities;

namespace Application.Features.ProductDiscounts.Dtos;

public record ProductDiscountDto
(
    Guid Id,
    Guid ProductId,
    string ProductName,
    string ProductBrand,
    string ProductImg,
    decimal ProductPrice,
    int DiscountPercentage,
    DateTime StartDate,
    DateTime EndDate,
    decimal DiscountedPrice
);