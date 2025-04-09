namespace CA_2025.Domain.Abstraction;

public abstract class Entity
{
    public Entity()
    {
        Id = Guid.NewGuid();
    }
    public Guid Id { get; set; }
    public DateTimeOffset CreateAt { get; set; }
    public DateTimeOffset? UpdateAt { get; set; }
    public DateTimeOffset? DeleteAt { get; set; }
    public bool IsDeleted { get; set; }
}
