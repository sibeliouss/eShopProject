using Domain.Entities;
using FluentValidation;

namespace Application.Features.Categories.Commands.Update;

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
   public UpdateCategoryCommandValidator()
   {
      RuleFor(c => c.Name).NotEmpty().MinimumLength(2).WithMessage("Kategori alanı boş olamaz!");
      RuleFor(c => c.IconImgUrl).NotEmpty();
   } 
}