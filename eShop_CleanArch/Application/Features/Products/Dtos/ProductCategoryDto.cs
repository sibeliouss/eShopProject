namespace Application.Features.Products.Dtos;

public record ProductCategoryDto
{
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; }
}