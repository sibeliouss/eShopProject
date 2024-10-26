using Application.Features.Reviews.Queries.Responses;
using Application.Services.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Reviews.Queries
{
    public class GetAllReviewsQuery : IRequest<List<GetAllReviewsQueryResponse>> 
    {
        public Guid ProductId { get; set; }

        public class GetReviewsQueryHandler : IRequestHandler<GetAllReviewsQuery, List<GetAllReviewsQueryResponse>>
        {
            private readonly IReviewRepository _reviewRepository;

            public GetReviewsQueryHandler(IReviewRepository reviewRepository)
            {
                _reviewRepository = reviewRepository;
            }

            public async Task<List<GetAllReviewsQueryResponse>> Handle(GetAllReviewsQuery request, CancellationToken cancellationToken)
            {
                var reviews = await _reviewRepository.Query()
                    .Where(r => r.ProductId == request.ProductId)
                    .Include(r => r.Product)
                    .Select(r => new GetAllReviewsQueryResponse
                    {
                        Id = r.Id,
                        ProductId = r.ProductId,
                        UserId = r.UserId,
                        Raiting = r.Rating,
                        Title = r.Title,
                        Comment = r.Comment,
                        CreateAt = r.CreateAt,
                        Product = r.Product 
                    })
                    .ToListAsync(cancellationToken);

                return reviews; 
            }
        }
    }
}