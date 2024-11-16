using Application.Features.Categories.Constants;
using Application.Services.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

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
        var categoryExists = await _categoryRepository.Query().Where(c => c.Name == categoryName).FirstOrDefaultAsync();
        if (categoryExists is not null)
        {
            throw new Exception(CategoryMessages.CategoryAlreadyExists);
        }
    }
    
}