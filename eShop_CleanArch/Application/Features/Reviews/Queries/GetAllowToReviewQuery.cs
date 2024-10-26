using Application.Features.Reviews.Queries.Responses;
using Application.Services.Orders;
using Application.Services.Repositories;
using MediatR;

namespace Application.Features.Reviews.Queries;

public class GetAllowToReviewQuery : IRequest<GetAllowToReviewQueryResponse>
{
    public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
    
    public GetAllowToReviewQuery(Guid productId, Guid userId)
    {
        ProductId = productId;
        UserId = userId;
    }
    
    public class GetAllowReviewQueryHandler : IRequestHandler<GetAllowToReviewQuery, GetAllowToReviewQueryResponse>
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IOrderService _orderService;

        public GetAllowReviewQueryHandler(IReviewRepository reviewRepository, IOrderService orderService)
        {
            _reviewRepository = reviewRepository;
            _orderService = orderService;
        }
        
        public async Task<GetAllowToReviewQueryResponse> Handle(GetAllowToReviewQuery request, CancellationToken cancellationToken)
        {
            var details = await _orderService.GetOrdersByUserAndProductAsync(request.UserId, request.ProductId);
            return new GetAllowToReviewQueryResponse
            {
                IsAllow = details.Count > 0,
                Message = details.Count > 0 ? "Yorum yapabilirsiniz." : "Bu ürünü yorumlamak için sipariş vermelisiniz."
            };

        }
    }
}