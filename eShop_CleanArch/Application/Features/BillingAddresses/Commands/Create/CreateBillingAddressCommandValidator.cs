using Application.Features.BillingAddresses.Dtos;
using FluentValidation;

namespace Application.Features.BillingAddresses.Commands.Create;

public class CreateBillingAddressCommandValidator : AbstractValidator<CreateBillingAddressDto>
{
    public CreateBillingAddressCommandValidator()
    {
        
        RuleFor(p => p.ContactName).NotEmpty().WithMessage("Contact name alanı boş bırakılamaz!");
        RuleFor(p => p.Country).NotEmpty().WithMessage("Zorunlu alan!");
        RuleFor(p => p.City).NotEmpty().WithMessage("Şehir ismi boş bırakılamaz!");
        RuleFor(p => p.Description).NotEmpty().WithMessage("Fatura adresi boş bırakılamaz!");
    }
}