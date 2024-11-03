using Domain.Entities.ValueObjects;

namespace Application.Features.Products.Queries.ResponseDtos;

public class GetProductResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Brand { get; set; }
    public string Img { get; set; }
    public Money Price { get; set; } = new(0, "₺");
    public bool IsFeatured { get; set; }
    public DateTime CreateAt { get; set; } = DateTime.Now;
    public Guid CategoryId { get; set; }
}