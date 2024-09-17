using Application.Services.Repositories;
using Domain.Entities;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class CategoryRepository : Repository<Category, AppDbContext>, ICategoryRepository
{
    public CategoryRepository(AppDbContext context) : base(context)
    {
    }
}