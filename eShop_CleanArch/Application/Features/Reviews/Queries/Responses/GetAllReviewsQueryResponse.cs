using Domain.Entities;

namespace Application.Features.Reviews.Queries.Responses;

public class GetAllReviewsQueryResponse
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public short Rating { get; set; }
    public string Title { get; set; }
    public string Comment { get; set; } 
    public DateTime CreateAt { get; set; } = DateTime.Now; 
}
