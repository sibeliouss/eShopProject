using Application.Services.Repositories;
using Domain.Entities;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class ProductCategoryRepository : Repository<ProductCategory, AppDbContext>, IProductCategoryRepository
{
    public ProductCategoryRepository(AppDbContext context) : base(context)
    {
    }
}