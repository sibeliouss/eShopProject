

namespace Application.Features.ProductDiscounts.Commands.Create;

public class CreatedProductDiscountResponse 
{
   public Guid Id { get; set; }
   public Guid ProductId { get; set; }
   public int DiscountPercentage { get; set; }
   public DateTime StartDate { get; set; }
   public DateTime EndDate { get; set; }
   public decimal DiscountedPrice { get; set; }
}