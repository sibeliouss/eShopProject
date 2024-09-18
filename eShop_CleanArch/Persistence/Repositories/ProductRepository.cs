using Application.Services.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class ProductRepository : Repository<Product, AppDbContext>,IProductRepository
{
    private readonly AppDbContext _context;
    private IDbContextTransaction _transaction;

    public ProductRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
    
    public async Task BeginTransactionAsync(CancellationToken cancellationToken)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken)
    {
        await _transaction.CommitAsync(cancellationToken);
    }
    
    public async Task RollbackTransactionAsync(CancellationToken cancellationToken)
    {
        await _transaction.RollbackAsync(cancellationToken);
    }

   
}