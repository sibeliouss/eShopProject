using Domain.Entities;
using Domain.Entities.ValueObjects;

namespace Application.Features.ProductDiscounts.Dtos;

public record ProductDiscountDto
(
    Guid Id,
    Guid ProductId,
   // Product Product,
    string Name,
    string Brand,
    string Img, 
    Money Price,
    int DiscountPercentage,
    DateTime StartDate,
    DateTime EndDate,
    decimal DiscountedPrice,
    int Quantity
);