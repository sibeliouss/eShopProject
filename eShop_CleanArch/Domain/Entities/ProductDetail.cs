using Domain.Abstract;

namespace Domain.Entities;

public class ProductDetail : Entity<Guid>
{
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    public string Description { get; set; }
    public string? Barcode { get; set; }
    public string Material { get; set; } 
    public string Size { get; set; } 
    public string Color { get; set; } 
    public string Fit { get; set; } 
    


}