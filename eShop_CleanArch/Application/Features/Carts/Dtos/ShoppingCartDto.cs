using System;
using Domain.Entities.ValueObjects;

namespace Application.Features.Baskets.Dtos;

public record ShoppingCartDto(
    Guid ProductId,
    Money Price,
    int Quantity,
    Guid UserId);