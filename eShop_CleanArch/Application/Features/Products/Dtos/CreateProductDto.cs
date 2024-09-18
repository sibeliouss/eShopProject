using Domain.Entities.ValueObjects;

namespace Application.Features.Products.Dtos;

public record CreateProductDto(
 string Name, 
 string Brand,
 string Img,
 string Description,
 Money Price,
 int Stock,
 string Barcode,
 bool IsFeatured,
List<Guid> CategoryIds);