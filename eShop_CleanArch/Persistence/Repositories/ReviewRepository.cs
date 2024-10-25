using Application.Services.Repositories;
using Domain.Entities;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class ReviewRepository : Repository<Review, AppDbContext>, IReviewRepository
{
    public ReviewRepository(AppDbContext context) : base(context)
    {
    }
}