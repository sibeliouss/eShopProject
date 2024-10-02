using Application.Features.ProductDetails.Dtos;
using Domain.Entities.ValueObjects;

namespace Application.Features.Products.Dtos;

public record CreateProductDto(
 string Name, 
 string Brand,
 string Img,
 int Quantity,
 Money Price,
 bool IsFeatured,
 List<Guid> CategoryIds);