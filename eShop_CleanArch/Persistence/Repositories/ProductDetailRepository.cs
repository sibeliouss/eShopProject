using Application.Services.Repositories;
using Domain.Entities;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class ProductDetailRepository : Repository<ProductDetail, AppDbContext>, IProductDetailRepository
{
    public ProductDetailRepository(AppDbContext context) : base(context)
    {
    }
}