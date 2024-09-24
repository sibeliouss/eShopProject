namespace Application.Features.ProductDetails.Commands.Create;

public class CreatedProductDetailResponse
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string? Description { get; set; }
    public int Stock { get; set; }
    public string? Barcode { get; set; }
    public string Material { get; set; } = string.Empty;
    public string Size { get; set; } = string.Empty;
    public string? Color { get; set; } = string.Empty;
    public string Fit { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
}