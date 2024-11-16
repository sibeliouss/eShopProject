using Domain.Entities;

namespace Application.Services.Products;

public interface IProductService
{
    IQueryable<Product> Query();
    
    Task<Product?> GetByIdAsync(Guid id);
    
    Task UpdateAsync(Product product);

    Task<Product?> FindAsync(Guid id);
    
    Task UpdateRangeAsync(IEnumerable<Product> products);  // Toplu güncelleme metodu
}