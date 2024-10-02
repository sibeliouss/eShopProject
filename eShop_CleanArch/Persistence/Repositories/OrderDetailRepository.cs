using Application.Services.Repositories;
using Domain.Entities;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class OrderDetailRepository : Repository<OrderDetail, AppDbContext>, IOrderDetailRepository
{
    public OrderDetailRepository(AppDbContext context) : base(context)
    {
    }
}