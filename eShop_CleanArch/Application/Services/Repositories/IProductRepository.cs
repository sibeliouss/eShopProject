using Domain.Entities;

namespace Application.Services.Repositories;

public interface IProductRepository :IRepository<Product>
{
    Task BeginTransactionAsync(CancellationToken cancellationToken);
    Task CommitTransactionAsync(CancellationToken cancellationToken);
    Task RollbackTransactionAsync(CancellationToken cancellationToken);
   
}