using FluentValidation;

namespace Application.Features.WishLists.Commands.Create;

public class CreateWishListCommandValidator : AbstractValidator<CreateWishListCommand>
{
    public CreateWishListCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Ürün ID boş olamaz.")
            .NotEqual(Guid.Empty).WithMessage("Geçerli bir ürün ID'si giriniz.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("Kullanıcı ID boş olamaz.")
            .NotEqual(Guid.Empty).WithMessage("Geçerli bir kullanıcı ID'si giriniz.");

        
 
    }
}