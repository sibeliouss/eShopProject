using Application.Features.Products.Constants;
using Application.Services.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Products.Rules;

public class ProductBusinessRules
{
    private readonly IProductRepository _productRepository;

    public ProductBusinessRules(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    
    private static void ThrowBusinessException(string message)
    {
        throw new BusinessException(message);
    }
    
    public async Task ProductNameAlreadyExists(string name)
    {
        var productExists = await _productRepository.Query().Where(p => p.Name == name).FirstOrDefaultAsync();
        if (productExists is not null)
        {
            ThrowBusinessException(ProductMessages.ProductAlreadyExists);
        }
    }
    
   
   
}

public class BusinessException(string message) : Exception(message);