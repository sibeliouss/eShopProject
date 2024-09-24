using Domain.Entities.ValueObjects;

namespace Application.Features.Products.Dtos;

public record CreateProductDto(
 string Name, 
 string Brand,
 string Img,
 Guid ProductDetailId,
 Money Price,
 bool IsFeatured,
List<Guid> CategoryIds);