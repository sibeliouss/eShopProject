using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.Services.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.ProductCategories;

public class ProductCategoryService : IProductCategoryService
{
    private readonly IProductCategoryRepository _productCategoryRepository;

    public ProductCategoryService(IProductCategoryRepository productCategoryRepository)
    {
        _productCategoryRepository = productCategoryRepository;
    }
    
    public async Task AddCategoriesToProductAsync(Guid productId, List<Guid> categoryIds)
    {
        foreach (var categoryId in categoryIds)
        {
            var productCategory = new ProductCategory()
            {
                Id = Guid.NewGuid(),
                ProductId = productId,
                CategoryId = categoryId
            };
            await _productCategoryRepository.AddAsync(productCategory);
        }
    }
    
    public async Task RemoveAllCategoriesFromProductAsync(Guid productId)
    {
        var existingCategories = await _productCategoryRepository.Query()
            .Where(pc => pc.ProductId == productId)
            .ToListAsync();

        foreach (var category in existingCategories)
        {
            await _productCategoryRepository.DeleteAsync(category);
        }
    }

    public async Task<ProductCategory?> GetByIdAsync(Guid id)
    {
        return await _productCategoryRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<ProductCategory>> GetAllAsync()
    {
        return await _productCategoryRepository.GetAllAsync();
    }

    public async Task AddAsync(ProductCategory category)
    {
        await _productCategoryRepository.AddAsync(category);
    }

    public async Task UpdateAsync(ProductCategory category)
    {
        await _productCategoryRepository.UpdateAsync(category);
    }

    public async Task DeleteAsync(ProductCategory category)
    {
        await _productCategoryRepository.DeleteAsync(category);
    }

    public async Task<bool> AnyAsync(Expression<Func<ProductCategory, bool>> predicate)
    {
        return await _productCategoryRepository.AnyAsync(predicate);
    }

    public IQueryable<ProductCategory> Query()
    {
        return _productCategoryRepository.Query();
    }
}