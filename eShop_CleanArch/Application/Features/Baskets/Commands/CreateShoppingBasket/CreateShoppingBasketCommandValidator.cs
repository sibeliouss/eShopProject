using Application.Features.Baskets.Dtos;
using FluentValidation;

namespace Application.Features.Baskets.Commands.CreateShoppingBasket;

public class CreateShoppingBasketCommandValidator : AbstractValidator<ShoppingBasketDto>
{
   public CreateShoppingBasketCommandValidator()
   {
      RuleFor(x => x.ProductId).NotEmpty();
      RuleFor(x => x.CustomerId).NotEmpty();
      RuleFor(x => x.Quantity).GreaterThan(0);
      RuleFor(x => x.Price).NotNull().Must(price => price.Value > 0); 
   }
}