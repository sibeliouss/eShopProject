using System;
using Domain.Entities.ValueObjects;

namespace Application.Features.Baskets.Dtos;

public record ShoppingBasketDto(
    Guid ProductId,
    Money Price,
    int Quantity,
    Guid UserId);