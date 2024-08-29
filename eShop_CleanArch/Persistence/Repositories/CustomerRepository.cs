using Application.Services.Repositories;
using Domain.Entities;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class CustomerRepository : Repository<Customer, AppDbContext>, ICustomerRepository
{
    public CustomerRepository(AppDbContext context) : base(context)
    {
    }
}