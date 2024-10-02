using Application.Features.Orders.Queries.Responses;
using Application.Services.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Orders.Queries;

public class GetAllOrdersByCustomerId : IRequest<List<GetAllOrdersByCustomerIdResponse>>
{
   public Guid CustomerId { get; set; }
   
   public class GetAllOrderByCustomerIdHandler : IRequestHandler<GetAllOrdersByCustomerId,List<GetAllOrdersByCustomerIdResponse> >
   {
      private readonly IOrderRepository _orderRepository;

      public GetAllOrderByCustomerIdHandler(IOrderRepository orderRepository)
      {
         _orderRepository = orderRepository;
      }
      public async Task<List<GetAllOrdersByCustomerIdResponse>> Handle(GetAllOrdersByCustomerId request, CancellationToken cancellationToken)
      {
         var orders = await _orderRepository.Query()
            .Where(o => o.CustomerId == request.CustomerId)
            .Include(o => o.OrderDetails).ThenInclude(p => p.Product)
            .ToListAsync(cancellationToken);
         
         if (orders.Count == 0)
         {
            throw new Exception("Kullanıcıya ait bir sipariş bulunamadı");
         }
         
         var orderResponse = orders.Select(o => new GetAllOrdersByCustomerIdResponse
         {
            Id = o.Id,
            OrderNumber = o.OrderNumber,
            CreateAt = o.CreateAt,
            ProductQuantity = o.ProductQuantity,
            Status = o.Status,
            PaymentCurrency = o.PaymentCurrency
         }).ToList();

         return orderResponse;
      }

   }
}