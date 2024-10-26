using Domain.Entities;

namespace Application.Services.Orders;

public interface IOrderService
{ 
    string GetNewOrderNumber(); 
    Task AddAsync(Order order);
    
    Task<List<Order>> GetOrdersByUserAndProductAsync(Guid userId, Guid productId);
}