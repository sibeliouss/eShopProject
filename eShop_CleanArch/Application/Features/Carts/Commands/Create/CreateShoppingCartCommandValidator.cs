using Application.Features.Baskets.Dtos;
using FluentValidation;

namespace Application.Features.Baskets.Commands.Create;

public class CreateShoppingCartCommandValidator : AbstractValidator<ShoppingCartDto>
{
   public CreateShoppingCartCommandValidator()
   {
      RuleFor(x => x.ProductId).NotEmpty();
      RuleFor(x => x.UserId).NotEmpty();
      RuleFor(x => x.Quantity).GreaterThan(0);
      RuleFor(x => x.Price).NotNull().Must(price => price.Value > 0); 
   }
}