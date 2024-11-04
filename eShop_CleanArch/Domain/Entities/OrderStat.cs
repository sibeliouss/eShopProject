using Domain.Abstract;
using Domain.Enums;

namespace Domain.Entities;

public class OrderStat : Entity<Guid>
{
    public string OrderNumber { get; set; } 
    public OrderStatusEnum StatusEnum { get; set; } = 0;
    public DateTime StatusDate { get; set; } = DateTime.Now;
}