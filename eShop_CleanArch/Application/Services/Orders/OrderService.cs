using Application.Services.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Orders;

    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public string GetNewOrderNumber()
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(Order order)
        {
            throw new NotImplementedException();
        }
        
        public async Task<List<Order>> GetOrdersByUserAndProductAsync(Guid userId, Guid productId)
        {
            return await _orderRepository.Query()
                .Include(o => o.OrderDetails) 
                .Where(o => o.UserId == userId && 
                            (o.Status == "Teslim Edildi") && 
                            o.OrderDetails.Any(od => od.ProductId == productId))
                .ToListAsync();
        }

      
    }

