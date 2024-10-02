using Application.Services.Repositories;
using Domain.Entities;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class BasketRepository : Repository<Basket, AppDbContext>, IBasketRepository
{
    public BasketRepository(AppDbContext context) : base(context)
    {
    }
}