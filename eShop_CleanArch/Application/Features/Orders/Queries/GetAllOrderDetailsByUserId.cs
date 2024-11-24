using Application.Features.Orders.Dtos;
using Application.Features.Orders.Helpers;
using Application.Features.Orders.Queries.Responses;
using Application.Features.Products.Dtos;
using Application.Services.Repositories;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Orders.Queries;

public class GetAllOrderDetailsByUserId : IRequest<GetAllOrderDetailsByUserIdResponse>
{
    public Guid UserId { get; set; }
    public Guid OrderId { get; set; }
    
    public class GetAllOrderDetailsByUserIdHandler : IRequestHandler<GetAllOrderDetailsByUserId, GetAllOrderDetailsByUserIdResponse>
    {
        private readonly IOrderRepository _orderRepository;

        public GetAllOrderDetailsByUserIdHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<GetAllOrderDetailsByUserIdResponse> Handle(GetAllOrderDetailsByUserId request, CancellationToken cancellationToken)
        {
            var currentDate = DateTime.Now; 
            var order = await _orderRepository.Query()
                .Where(o => o.UserId == request.UserId && o.Id == request.OrderId)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product).ThenInclude(od=>od.ProductDiscount)
                .FirstOrDefaultAsync(cancellationToken); 

            if (order == null)
            {
                throw new Exception("Kullanıcıya ait herhangi bir sipariş bulunamadı.");
            }
            
            var orderResponse = new GetAllOrderDetailsByUserIdResponse
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                ProductQuantity = order.ProductQuantity,
                CreateAt = order.CreateAt,
                PaymentMethod = order.PaymentMethod,
                Status = order.Status,
                //Status = OrderStatusHelper.GetLocalizedStatus(order.Status, "tr"),
                PaymentCurrency = order.PaymentCurrency,
                PaymentNumber = order.PaymentNumber,
                Products = order.OrderDetails.Select(od => new OrderDetailDto
                {
                    ProductId = od.ProductId,
                    Quantity = od.Quantity,
                    Brand = od.Product.Brand,
                    Name = od.Product.Name,
                    ProductDiscount = od.Product.ProductDiscount != null
                        ? new ProductDiscountDto()
                        {
                            Id = od.Product.ProductDiscount.Id,
                            ProductId = od.Product.Id,
                            DiscountedPrice = od.Product.ProductDiscount.DiscountedPrice,
                        }
                        : null,
                    Price = od.Price
                }).ToList()
            };

            return orderResponse;
        }
    }
}
