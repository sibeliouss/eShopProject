using Domain.Abstract;

namespace Domain.Entities;

public class ProductDetail : Entity<Guid>
{
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    public string? Description { get; set; }
    public int Stock { get; set; }
    public string? Barcode { get; set; }
    public string Material { get; set; } = string.Empty;
    public string Size { get; set; } = string.Empty;
    public string? Color { get; set; } = string.Empty;
    public string Fit { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;


}