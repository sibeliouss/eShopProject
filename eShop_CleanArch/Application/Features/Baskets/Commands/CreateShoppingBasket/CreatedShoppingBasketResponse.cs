using Domain.Entities.ValueObjects;

namespace Application.Features.Baskets.Commands.CreateShoppingBasket;

public class CreatedShoppingBasketResponse
{
   public Guid ProductId { get; set; }
   public Money Price { get; set; }
   public int Quantity { get; set; }
   public Guid CustomerId { get; set; }
}