using Domain.Abstract;
using Domain.Enums;

namespace Domain.Entities;

public class Order : Entity<Guid>
{
   public Guid UserId { get; set; }
   public User? User { get; set; }
   public string OrderNumber { get; set; } 
   public int ProductQuantity { get; set; }
   public DateTime PaymentDate { get; set; }=DateTime.Now;
   public string PaymentMethod { get; set; } 
   
   public string PaymentNumber { get; set; } 
   public string Status { get; set; }
   public string PaymentCurrency { get; set; } 
   public ICollection<OrderDetail> OrderDetails { get; set; }
}