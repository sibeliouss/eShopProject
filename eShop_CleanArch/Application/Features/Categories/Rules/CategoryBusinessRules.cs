using Application.Features.Categories.Constants;
using Application.Services.Repositories;
using Domain.Entities;

namespace Application.Features.Categories.Rules;

public class CategoryBusinessRules
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryBusinessRules(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task CategoryNameAlreadyExists(string categoryName)
    {
        var categoryExists = await _categoryRepository.AnyAsync(c => c.Name == categoryName);
        if (categoryExists)
        {
            throw new Exception(CategoryMessages.CategoryAlreadyExists);
        }
    }
    
}