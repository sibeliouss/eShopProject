using Application.Services.Repositories;
using Domain.Entities;

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
}