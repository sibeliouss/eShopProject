using Domain.Entities.ValueObjects;

namespace Application.Features.Products.Dtos;

public record UpdateProductDto(
    Guid Id,
    string Name, 
    string Brand,
    string Img,
    int Quantity,
    Money Price,
    bool IsFeatured,
    List<Guid> CategoryIds);