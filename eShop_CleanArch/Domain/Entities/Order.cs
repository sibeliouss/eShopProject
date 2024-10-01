using Domain.Abstract;
using Domain.Enums;

namespace Domain.Entities;

public class Order : Entity<Guid>
{
   public Guid CustomerId { get; set; }
   public Customer? Customer { get; set; }
   public string OrderNumber { get; set; } = string.Empty;
   public int ProductQuantity { get; set; }
   public DateTime PaymentDate { get; set; }=DateTime.Now;
   public string PaymentMethod { get; set; } = string.Empty;
   public string Status { get; set; } = string.Empty;
   public string PaymentCurrency { get; set; } = string.Empty;
   public ICollection<OrderDetail> OrderDetails { get; set; }
}