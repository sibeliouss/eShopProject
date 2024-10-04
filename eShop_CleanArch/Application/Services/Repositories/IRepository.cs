using System.Linq.Expressions;

namespace Application.Services.Repositories;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    IQueryable<T> Query();
    Task UpdateRangeAsync(IEnumerable<T> entities);
    Task DeleteRangeAsync(IEnumerable<T> entities);

}