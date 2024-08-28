namespace Domain.Abstract;

public interface IEntity<TId>
{
    public TId Id { get; set; }
    public DateTime CreateAt { get; set; }
    public DateTime? UpdateAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}
public interface IEntity : IEntity<Guid> { }