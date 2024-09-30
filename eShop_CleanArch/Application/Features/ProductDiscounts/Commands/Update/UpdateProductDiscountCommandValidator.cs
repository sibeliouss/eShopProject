using Application.Features.ProductDiscounts.Dtos;
using FluentValidation;

namespace Application.Features.ProductDiscounts.Commands.Update;

public class UpdateProductDiscountCommandValidator : AbstractValidator<UpdateProductDiscountDto>
{
    public UpdateProductDiscountCommandValidator()
    {
        RuleFor(x => x.DiscountPercentage).InclusiveBetween(0, 100).WithMessage("İndirim yüzdesi 0 ile 100 arasında olmalıdır.");
        RuleFor(x => x.StartDate)
            .LessThan(x => x.EndDate).WithMessage("Başlangıç tarihi, bitiş tarihinden önce olmalıdır.");
        RuleFor(x => x.EndDate).GreaterThan(DateTime.Now);
    }
}