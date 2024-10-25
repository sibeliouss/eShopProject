using Application.Services.Repositories;
using Domain.Entities;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class WishListRepository : Repository<WishList, AppDbContext>, IWishListRepository
{
    public WishListRepository(AppDbContext context) : base(context)
    {
    }
}