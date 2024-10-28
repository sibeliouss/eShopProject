using Domain.Entities.ValueObjects;

namespace Application.Features.Carts.Commands.Create;

public class CreatedShoppingCartResponse
{
   public Guid ProductId { get; set; }
   public Money Price { get; set; }
   public int Quantity { get; set; }
   public Guid UserId { get; set; }
}