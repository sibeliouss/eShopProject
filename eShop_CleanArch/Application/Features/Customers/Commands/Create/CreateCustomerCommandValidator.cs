using Application.Features.Customers.Dtos;
using FluentValidation;

namespace Application.Features.Customers.Commands.Create;

public class CreateCustomerCommandValidator :  AbstractValidator<CreateCustomerDto>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(p => p.FirstName).NotEmpty().WithMessage("İsim alanı boş alamz!");
        RuleFor(p => p.FirstName).MinimumLength(3).WithMessage("Kullanıcı adı en az 3 karakter olmalıdır");

        RuleFor(p => p.LastName).NotEmpty().WithMessage("Soyadı boş olamaz!");
        RuleFor(p => p.LastName).MinimumLength(3).WithMessage("Geçerli bir soyad girin!");

        RuleFor(p => p.Email).NotEmpty().WithMessage("E-posta boş olamaz!");
        RuleFor(p => p.Email).MinimumLength(3).WithMessage("Geçerli bir e-posta adresi girin");
        RuleFor(p => p.Email).Matches(".+@.+").WithMessage("E-posta adresi '@' işareti içermelidir");

        RuleFor(p => p.UserName).NotEmpty().WithMessage("Kullanıcı adı boş olamaz!");
        RuleFor(p => p.UserName).MinimumLength(3).WithMessage("Geçerli bir kullanıcı adı girin");

    }
}