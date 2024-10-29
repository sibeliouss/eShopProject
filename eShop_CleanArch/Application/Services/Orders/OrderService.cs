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
        
        // sipariş numarası SBL + yıl + 00000000x 
        public string GetNewOrderNumber()
        {
            const string initialLetter = "SBL";
            var year = DateTime.Now.Year.ToString();
            var newOrderNumber = initialLetter + year;
            
            var lastOrder = _orderRepository.Query().OrderByDescending(o => o.Id).FirstOrDefault();
            
            if (lastOrder is not null)
            {
                var currentOrderNumber = lastOrder.OrderNumber;
                // Son siparişin yılı ile şimdiki yılı karşılaştır
                var currentYear = currentOrderNumber.Substring(2, 4);
                // Eğer yıllar aynıysa mevcut sipariş numarasını artırmak için başlangıç noktası
                var startIndex = (currentYear == year) ? 7 : 0;
                GenerateUniqueOrderNumber(ref newOrderNumber, currentOrderNumber[startIndex..]);
            }
            else
            {
                newOrderNumber += "000000001";
            }
            
            return newOrderNumber;
        }

        public async Task AddAsync(Order order)
        {
            await _orderRepository.AddAsync(order);
        }

        private void GenerateUniqueOrderNumber(ref string newOrderNumber, string currentOrderNumStr)
        {
            var currentOrderNumberInt = int.TryParse(currentOrderNumStr, out var num) ? num : 0;
            var isOrderNumberUnique = false;

            // Benzersiz bir sipariş numarası bulunana kadar döngü
            while (!isOrderNumberUnique)
            {
                
                currentOrderNumberInt++;
                var potentialOrderNumber = newOrderNumber + currentOrderNumberInt.ToString("D9");
                
                var order = _orderRepository.Query().FirstOrDefault(o => o.OrderNumber == potentialOrderNumber);
                if (order == null)
                {
                    newOrderNumber = potentialOrderNumber; 
                    isOrderNumberUnique = true; // Döngüden çıkmak için 
                }
            }
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

