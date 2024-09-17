using Application.Services.Repositories;
using Domain.Entities;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class ProductRepository : Repository<Product, AppDbContext>,IProductRepository
{
    public ProductRepository(AppDbContext context) : base(context)
    {
    }
}