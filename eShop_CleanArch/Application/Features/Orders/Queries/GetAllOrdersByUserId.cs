using Application.Features.Orders.Helpers;
using Application.Features.Orders.Queries.Responses;
using Application.Services.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Orders.Queries;

public class GetAllOrdersByUserId : IRequest<List<GetAllOrdersByUserIdResponse>>
{
   public Guid UserId { get; set; }
   
   public class GetAllOrderByUserIdHandler : IRequestHandler<GetAllOrdersByUserId,List<GetAllOrdersByUserIdResponse> >
   {
      private readonly IOrderRepository _orderRepository;

      public GetAllOrderByUserIdHandler(IOrderRepository orderRepository)
      {
         _orderRepository = orderRepository;
      }
      public async Task<List<GetAllOrdersByUserIdResponse>> Handle(GetAllOrdersByUserId request, CancellationToken cancellationToken)
      {
         var orders = await _orderRepository.Query()
            .Where(o => o.UserId == request.UserId)
            .Include(o => o.OrderDetails).ThenInclude(p => p.Product)
            .ToListAsync(cancellationToken);
         
         if (orders.Count == 0)
         {
            throw new Exception("Kullanıcıya ait bir sipariş bulunamadı");
         }
         
         var orderResponse = orders.Select(o => new GetAllOrdersByUserIdResponse
         {
            Id = o.Id,
            OrderNumber = o.OrderNumber,
            CreateAt = o.CreateAt,
            ProductQuantity = o.ProductQuantity,
            Status = o.Status,
            // Status = OrderStatusHelper.GetLocalizedStatus(o.Status, "tr"),
            PaymentCurrency = o.PaymentCurrency
         }).ToList();

         return orderResponse;
      }

   }
}