using Application.Features.Products.Dtos;
using FluentValidation;

namespace Application.Features.Products.Commands.Update;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductDto>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(product => product.Id).NotEmpty().WithMessage("Ürün ID'si boş olamaz.");

        RuleFor(product => product.Name).NotEmpty().WithMessage("Ürün adı boş olamaz.")
            .Length(2, 100).WithMessage("Ürün adı 2 ile 100 karakter arasında olmalıdır.");

        RuleFor(product => product.Brand)
            .NotEmpty().WithMessage("Marka boş olamaz.")
            .Length(2, 50).WithMessage("Marka 2 ile 50 karakter arasında olmalıdır.");

        RuleFor(product => product.Img)
            .NotEmpty().WithMessage("Resim URL'si boş olamaz.");

        RuleFor(product => product.Price)
            .NotNull().WithMessage("Fiyat boş olamaz.")
            .Must(price => price.Value > 0).WithMessage("Fiyat 0'dan büyük olmalıdır.");
        
     
      
    }
}