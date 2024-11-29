using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
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
        private readonly IMapper _mapper;
        private readonly IValidator<CreateReviewCommand> _validator;


        public CreateReviewCommandHandler(IReviewRepository reviewRepository, IMapper mapper, IValidator<CreateReviewCommand> validator)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
            _validator = validator;
        }
        
        public async Task Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);
            var existingReview = await _reviewRepository.Query()
                .Where(r => r.UserId == request.UserId && r.ProductId == request.ProductId)
                .FirstOrDefaultAsync(cancellationToken);
                
            if (existingReview != null)
            {
                throw new Exception("Bu ürün için zaten yorumunuz bulunmaktadır.");
            }
            
            var review = _mapper.Map<Review>(request);

            await _reviewRepository.AddAsync(review);
        }
    }
}