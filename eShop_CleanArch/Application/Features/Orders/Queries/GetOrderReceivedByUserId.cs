using Application.Features.Orders.Dtos;
using Application.Features.Orders.Helpers;
using Application.Features.Orders.Queries.Responses;
using Application.Services.Repositories;
using Domain.Entities.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Orders.Queries;

public class GetOrderReceivedByUserId : IRequest<GetOrderReceivedByUserIdResponse>
{
    public Guid UserId { get; set; }
    
    public class GetOrderReceivedByCustomerIdHandler : IRequestHandler<GetOrderReceivedByUserId, GetOrderReceivedByUserIdResponse>
    {
        private readonly IOrderRepository _orderRepository;

        public GetOrderReceivedByCustomerIdHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task<GetOrderReceivedByUserIdResponse> Handle(GetOrderReceivedByUserId request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.Query().Where(o => o.UserId == request.UserId)
                .OrderByDescending(o => o.CreateAt).Include(o => o.OrderDetails).ThenInclude(o => o.Product)
                .FirstOrDefaultAsync(cancellationToken);
            if (order is null) throw new Exception("Müşteriye ait herhangi bir sipariş bulunamadı!");

            var orderResponse = new GetOrderReceivedByUserIdResponse()
            {
                Id = order.Id,
                CreateAt = order.CreateAt,
                OrderNumber = order.OrderNumber,
                ProductQuantity = order.ProductQuantity,
                PaymentCurrency = order.PaymentCurrency,
                PaymentMethod = order.PaymentMethod,
                PaymentNumber = order.PaymentNumber,
                Status = order.Status,
                // Status = OrderStatusHelper.GetLocalizedStatus(order.Status, "tr"),
                Products = order.OrderDetails.Select(orderDetail => new OrderDetailDto()
                {
                    ProductId = orderDetail.ProductId,
                    Name = orderDetail.Product.Name,
                    Brand = orderDetail.Product.Brand,
                    Quantity = orderDetail.Quantity,
                    Price = new Money(orderDetail.Price.Value, orderDetail.Price.Currency)
                }).ToList()
            };
                return orderResponse;

            
        }
    }
}