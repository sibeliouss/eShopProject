using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Services.ProductCategories;

public interface IProductCategoryService
{
    Task AddCategoriesToProductAsync(Guid productId, List<Guid> categoryIds);
    Task RemoveAllCategoriesFromProductAsync(Guid productId);
    Task<ProductCategory?> GetByIdAsync(Guid id);
    Task<IEnumerable<ProductCategory>> GetAllAsync();
    Task AddAsync(ProductCategory category);
    Task UpdateAsync(ProductCategory category);
    Task DeleteAsync(ProductCategory category);
    Task<bool> AnyAsync(Expression<Func<ProductCategory, bool>> predicate);
    IQueryable<ProductCategory> Query();
}