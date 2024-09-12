using Application.Services.Repositories;
using Domain.Entities;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class BillingAddressRepository : Repository<BillingAddress, AppDbContext>,IBillingAddressRepository
{
    public BillingAddressRepository(AppDbContext context) : base(context)
    {
    }
}