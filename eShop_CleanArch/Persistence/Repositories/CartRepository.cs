using Application.Services.Repositories;
using Domain.Entities;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class CartRepository : Repository<Cart, AppDbContext>, ICartRepository
{
    public CartRepository(AppDbContext context) : base(context)
    {
    }
}