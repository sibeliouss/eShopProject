using FluentValidation;

namespace Application.Features.Categories.Commands.Create;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
   public CreateCategoryCommandValidator()
   {
      RuleFor(c => c.Name).NotEmpty().MinimumLength(2).WithMessage("Kategori alanı boş olamaz!");
      RuleFor(c => c.IconImgUrl).NotEmpty();
   } 
}