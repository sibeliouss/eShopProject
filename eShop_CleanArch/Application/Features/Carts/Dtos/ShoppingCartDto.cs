using Domain.Entities.ValueObjects;

namespace Application.Features.Carts.Dtos;

public record ShoppingCartDto(
    Guid ProductId,
    Money Price,
    int Quantity,
    Guid UserId);