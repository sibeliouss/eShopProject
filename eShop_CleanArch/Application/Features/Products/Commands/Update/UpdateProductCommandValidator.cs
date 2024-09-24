using Application.Features.Products.Dtos;
using FluentValidation;

namespace Application.Features.Products.Commands.Update;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductDto>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(p => p.Name).NotEmpty();
        RuleFor(p => p.Brand).NotEmpty();
        RuleFor(p => p.Img).NotEmpty();
        RuleFor(p => p.Price).NotEmpty();
     
      
    }
}