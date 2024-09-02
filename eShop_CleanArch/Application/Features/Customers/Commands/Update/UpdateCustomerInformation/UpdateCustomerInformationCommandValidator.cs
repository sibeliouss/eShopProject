using Application.Features.Customers.Dtos;
using FluentValidation;

namespace Application.Features.Customers.Commands.Update.UpdateCustomerInformation;

public class UpdateCustomerInformationCommandValidator: AbstractValidator<UpdateCustomerInformationDto>
{
    public UpdateCustomerInformationCommandValidator()
    {
        RuleFor(p => p.FirstName).NotEmpty().WithMessage("Ad alanı boş olamaz!");
        RuleFor(p => p.FirstName).MinimumLength(3).WithMessage("En az 3 karakter olmalıdır");

        RuleFor(p => p.LastName).NotEmpty().WithMessage("Soyadalanı boş olamaz!");
        RuleFor(p => p.LastName).MinimumLength(3).WithMessage("Geçerli bir soyad girin!");

        RuleFor(p => p.Email).NotEmpty().WithMessage("Geçerli bir e-posta adresi girin");
        RuleFor(p => p.Email).MinimumLength(3).WithMessage("Geçerli bir e-posta adresi girin");
        RuleFor(p => p.Email).Matches(".+@.+").WithMessage("E-posta adresi '@' işareti içermelidir");

        RuleFor(p => p.UserName).NotEmpty().WithMessage("Geçerli bir kullanıcı adı girin!");
        RuleFor(p => p.UserName).MinimumLength(3).WithMessage("Geçerli bir kullanıcı adı girin!");
    }

}