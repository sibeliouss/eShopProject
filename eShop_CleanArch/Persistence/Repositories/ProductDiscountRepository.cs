using Application.Services.Repositories;
using Domain.Entities;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class ProductDiscountRepository :Repository<ProductDiscount,AppDbContext>,IProductDiscountRepository
{
    public ProductDiscountRepository(AppDbContext context) : base(context)
    {
    }
}