namespace Application.Features.ProductDetails.Dtos;

public class CreateProductDetailDto
{
    public Guid ProductId { get; set; }
    public string? Description { get; set; }
    public string? Barcode { get; set; }
    public string Material { get; set; } = string.Empty;
    public string Size { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public string Fit { get; set; } = string.Empty;
   
}