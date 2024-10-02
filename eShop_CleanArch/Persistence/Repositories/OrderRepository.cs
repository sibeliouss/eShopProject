using Application.Services.Repositories;
using Domain.Entities;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class OrderRepository : Repository<Order, AppDbContext>, IOrderRepository
{
    public OrderRepository(AppDbContext context) : base(context)
    {
    }
}