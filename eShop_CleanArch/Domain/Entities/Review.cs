using Domain.Abstract;

namespace Domain.Entities;

public class Review : Entity<Guid>
{
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public short Rating { get; set; }
    public string? Title { get; set; }
    public string? Comment { get; set; } 
   
}