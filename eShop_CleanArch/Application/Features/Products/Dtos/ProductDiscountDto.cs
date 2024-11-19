namespace Application.Features.Products.Dtos;

public class ProductDiscountDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public int DiscountPercentage { get; set; } = 0;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal DiscountedPrice { get; set; } = 0;
}