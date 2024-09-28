using Application.Features.ProductDetails.Dtos;
using FluentValidation;

namespace Application.Features.ProductDetails.Commands.Update;

public class UpdateProductDetailCommandValidator : AbstractValidator<UpdateProductDetailDto>
{
    public UpdateProductDetailCommandValidator()
    {
        RuleFor(x => x.Barcode).NotEmpty().WithMessage("Barkod boş olamaz.");
        RuleFor(x => x.Color).NotEmpty().WithMessage("Renk bilgisi boş olamaz.");
        RuleFor(x => x.Stock).GreaterThanOrEqualTo(0).WithMessage("Stok miktarı negatif olamaz.");
        RuleFor(x => x.Size).NotEmpty().WithMessage("Beden bilgisi boş olamaz.");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Açıklama boş olamaz.");
        RuleFor(x => x.Material).NotEmpty();
    }
}