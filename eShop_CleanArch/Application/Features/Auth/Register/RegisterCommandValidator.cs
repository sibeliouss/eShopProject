using Application.Features.Customers.Dtos;
using FluentValidation;

namespace Application.Features.Auth.Register;

public class RegisterCommandValidator: AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(p => p.FirstName).NotEmpty().WithMessage("İsim alanı boş olamaz!");
        RuleFor(p => p.FirstName).MinimumLength(3).WithMessage("En az 3 karakter olmalıdır");

        RuleFor(p => p.LastName).NotEmpty().WithMessage("Soyadı boş olamaz!");
        RuleFor(p => p.LastName).MinimumLength(3).WithMessage("Geçerli bir soyad girin!");

        RuleFor(p => p.Email).NotEmpty().WithMessage("E-posta alanı boş olamaz!");
        RuleFor(p => p.Email).MinimumLength(3).WithMessage("Geçerli bir e-posta adresi girin");
        RuleFor(p => p.Email).Matches(".+@.+").WithMessage("E-posta adresi '@' işareti içermelidir");

        RuleFor(p => p.UserName).NotEmpty().WithMessage("Kullanıcı adı boş olamaz!");
        RuleFor(p => p.UserName).MinimumLength(3).WithMessage("Geçerli bir kullanıcı adı girin");

        RuleFor(p => p.Password).NotEmpty().WithMessage("Şifre alanı boş olamaz");
        RuleFor(p => p.Password).Matches("[A-Z]").WithMessage("Şifre en az 1 adet büyük harf içermelidir!");
        RuleFor(p => p.Password).Matches("[a-z]").WithMessage("Şifre en az 1 adet küçük harf içermelidir!");
        RuleFor(p => p.Password).Matches("[0-9]").WithMessage("Şifre en az 1 rakam içermelidir!");
        RuleFor(p => p.Password).Matches("[^a-zA-Z0-9]").WithMessage("Şifre en az 1 adet özel karakter içermelidir!");

        RuleFor(p => p.ConfirmedPassword).NotEmpty().WithMessage("Doğrulanmış şifre alanı boş olamaz");
        RuleFor(p => p.ConfirmedPassword).Matches("[A-Z]").WithMessage("Doğrulanmış şifre en az 1 adet büyük harf içermelidir!");
        RuleFor(p => p.ConfirmedPassword).Matches("[a-z]").WithMessage("Doğrulanmış şifre en az 1 adet küçük harf içermelidir!");
        RuleFor(p => p.ConfirmedPassword).Matches("[0-9]").WithMessage("Doğrulanmış şifre en az 1 rakam içermelidir!");
        RuleFor(p => p.ConfirmedPassword).Matches("[^a-zA-Z0-9]").WithMessage("Doğrulanmış şifre en az 1 adet özel karakter içermelidir!");

    }

}
