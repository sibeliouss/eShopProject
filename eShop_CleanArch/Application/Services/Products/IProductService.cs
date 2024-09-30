using Domain.Entities;

namespace Application.Services.Products;

public interface IProductService
{
    IQueryable<Product> Query();
}