using Domain.Entities.ValueObjects;

namespace Application.Features.Products.Commands.Update;

public class UpdatedProductResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Brand { get; set; }
    public int Quantity { get; set; }
    public string Img { get; set; }
    public Money Price { get; set; } = new(0, "TRY");
    public bool IsFeatured { get; set; }
   
}