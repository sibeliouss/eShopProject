using Application.Features.Reviews.Queries.Responses;
using Application.Services.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Reviews.Queries;

public class CalculateStarQuery : IRequest<CalculateStarResponse>
{
    public Guid ProductId { get; set; }
    
    public class CalculateStarQueryHandler : IRequestHandler<CalculateStarQuery, CalculateStarResponse>
    {
        private readonly IReviewRepository _reviewRepository;

        public CalculateStarQueryHandler(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }
        public async Task<CalculateStarResponse> Handle(CalculateStarQuery request, CancellationToken cancellationToken)
        {
            var reviews = await _reviewRepository.Query().Where(r => r.ProductId == request.ProductId)
                .Select(r => r.Rating).ToListAsync(cancellationToken);
            
            var star1 = 0;
            var star2 = 0;
            var star3 = 0;
            var star4 = 0;
            var star5 = 0;

            foreach (var rating in reviews)
            {
                if (rating == 1) star1++;
                else if (rating == 2) star2++;
                else if (rating == 3) star3++;
                else if (rating == 4) star4++;
                else if (rating == 5) star5++;
            }

            return new CalculateStarResponse
            {
                Star1 = star1,
                Star2 = star2,
                Star3 = star3,
                Star4 = star4,
                Star5 = star5
            };
        }
    }
}