using Application.Services.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Products;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productService)
    {
        _productRepository = productService;
    }
    public IQueryable<Product> Query()
    {
        return _productRepository.Query();
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await _productRepository.GetByIdAsync(id);
    }
    
    public async Task<Product?> FindAsync(Guid id)
    {
        return await _productRepository.FindAsync(id); 
    }
    
  

    public async Task UpdateAsync(Product product)
    {
        await _productRepository.UpdateAsync(product);
    }
    
    public async Task UpdateRangeAsync(IEnumerable<Product> products)
    {
        await _productRepository.UpdateRangeAsync(products);  // IProductRepository'deki metodu çağırıyoruz
    }

}