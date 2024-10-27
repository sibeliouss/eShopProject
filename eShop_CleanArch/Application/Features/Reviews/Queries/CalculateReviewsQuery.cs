using Application.Services.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Reviews.Queries;

public class CalculateReviewsQuery : IRequest<double>
{
    public Guid ProductId { get; set; }

    public CalculateReviewsQuery(Guid productId)
    {
        ProductId = productId;
    }
    
    public class CalculateReviewsHandler : IRequestHandler<CalculateReviewsQuery, double>
    {
        private readonly IReviewRepository _reviewRepository;

        public CalculateReviewsHandler(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }
        public async Task<double> Handle(CalculateReviewsQuery request, CancellationToken cancellationToken)
        {
            var reviews = await _reviewRepository.Query().Where(r => r.ProductId == request.ProductId)
                .Select(r => r.Rating).ToListAsync(cancellationToken);
            
            if (reviews.Count == 0)
            {
                throw new KeyNotFoundException("Belirtilen ürün için yorum bulunamadı."); // NotFound için bir exception fırlat
            }

            double sum = 0;
            foreach (var rating in reviews)
            {
                sum += rating;
            }

            var averageRating = sum / reviews.Count;
            return averageRating;
        }
    }
}