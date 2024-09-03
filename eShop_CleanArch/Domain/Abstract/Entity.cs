namespace Domain.Abstract;

public class Entity<TId> : IEntity<TId>
{
    public TId Id { get; set; }
    public DateTime CreateAt { get; set; }
    public DateTime? UpdateAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}