using Application.Services.Repositories;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Reviews.Commands.Create;

public class CreateReviewCommand : IRequest
{
    public Guid ProductId { get; set; }
    public Guid UserId { get; set; }
    public short Rating { get; set; } 
    public string Title { get; set; }
    public string Comment { get; set; }
    public DateTime CreateAt { get; set; } = DateTime.Now;
    
    public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand>
    {
        private readonly IReviewRepository _reviewRepository;

        public CreateReviewCommandHandler(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }
        
        public async Task Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            var existingReview = await _reviewRepository.Query()
                .Where(r => r.UserId == request.UserId && r.ProductId == request.ProductId)
                .FirstOrDefaultAsync(cancellationToken);
                
            if (existingReview != null)
            {
                throw new Exception("Bu ürün için zaten yorumunuz bulunmaktadır.");
            }
            
            
            var review = new Review
            {
                ProductId  = request.ProductId, 
                UserId = request.UserId,
                Comment = request.Comment,
                Rating = request.Rating,
                Title = request.Title,
                CreateAt = request.CreateAt,
            };

            
            await _reviewRepository.AddAsync(review);
        }
    }
}