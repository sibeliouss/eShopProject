namespace Application.Features.ProductDetails.Queries;

public class GetAllProductDetailResponse
{
    public Guid Id { get; set; }
    public string? Description { get; set; }
    public string? Barcode { get; set; }
    public string Material { get; set; } = string.Empty;
    public string Size { get; set; } = string.Empty;
    public string? Color { get; set; } = string.Empty;
    public string Fit { get; set; } = string.Empty;
   
}