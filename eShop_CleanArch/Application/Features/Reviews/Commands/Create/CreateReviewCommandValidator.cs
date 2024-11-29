using FluentValidation;

namespace Application.Features.Reviews.Commands.Create;

public class CreateReviewCommandValidator : AbstractValidator<CreateReviewCommand>
{
    public CreateReviewCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("Ürün ID boş olamaz.").NotEqual(Guid.Empty).WithMessage("Geçersiz Ürün ID.");
        RuleFor(x => x.UserId).NotEmpty().WithMessage("Kullanıcı ID boş olamaz.").NotEqual(Guid.Empty).WithMessage("Geçersiz Kullanıcı ID.");
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Başlık boş olamaz.")
            .MaximumLength(100).WithMessage("Başlık en fazla 100 karakter olmalıdır.");
        RuleFor(x => x.Comment)
            .NotEmpty().WithMessage("Yorum boş olamaz.")
            .MaximumLength(1000).WithMessage("Yorum en fazla 1000 karakter olmalıdır.");

    }
}