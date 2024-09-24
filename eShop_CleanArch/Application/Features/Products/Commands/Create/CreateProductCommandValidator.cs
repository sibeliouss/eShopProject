using Application.Features.Products.Dtos;
using FluentValidation;

namespace Application.Features.Products.Commands.Create;

public class CreateProductCommandValidator : AbstractValidator<CreateProductDto>
{
    public CreateProductCommandValidator()
    {
        RuleFor(p => p.Name).NotEmpty();
        RuleFor(p => p.Brand).NotEmpty();
        RuleFor(p => p.Img).NotEmpty();
        RuleFor(p => p.Price).NotEmpty();
    }
}