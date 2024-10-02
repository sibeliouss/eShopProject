using Domain.Entities.ValueObjects;

namespace Application.Features.Orders.Dtos;

public class OrderDetailDto
{
    public Guid ProductId { get; set; }
    public string Name { get; set; }
    public string Brand { get; set; }
    public int Quantity { get; set; }
    public Money Price { get; set; } = new(0, "₺");
}