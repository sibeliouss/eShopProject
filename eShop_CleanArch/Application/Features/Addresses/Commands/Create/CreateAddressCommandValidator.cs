using FluentValidation;

namespace Application.Features.Addresses.Commands.Create;

public class CreateAddressCommandValidator : AbstractValidator<CreateAddressCommand>
{
   public CreateAddressCommandValidator()
   {
      RuleFor(p => p.ContactName).NotEmpty().WithMessage("Contact name alanı boş bırakılamaz!");
      RuleFor(p => p.Country).NotEmpty().WithMessage("Zorunlu alan");
      RuleFor(p => p.City).NotEmpty().WithMessage("Şehir ismi boş bırakılamaz!");
      RuleFor(p => p.Description).NotEmpty().WithMessage("Adres açıklaması zorunludur");
   } 
}