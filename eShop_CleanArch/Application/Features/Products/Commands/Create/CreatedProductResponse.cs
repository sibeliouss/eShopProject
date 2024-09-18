using Domain.Entities.ValueObjects;

namespace Application.Features.Products.Commands.Create;

public class CreatedProductResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Brand { get; set; }
    public string Img { get; set; }
    public string? Description { get; set; }
    public Money Price { get; set; } = new(0, "₺");
    public int Stock { get; set; }
    public string Barcode { get; set; }
    public bool IsFeatured { get; set; }
    
    public bool IsActive { get; set; } = true;
}