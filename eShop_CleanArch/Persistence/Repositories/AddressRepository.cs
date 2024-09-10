using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System.Linq.Expressions;
using Application.Services.Repositories;

namespace Persistence.Repositories;

public class AddressRepository : Repository<Address, AppDbContext>, IAddressRepository
{
    private readonly AppDbContext _context;

    public AddressRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
    
}