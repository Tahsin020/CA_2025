namespace CA_2025.Domain.Abstraction;

public abstract class EntityDto
{
    public Guid Id { get; set; }
    public DateTime CreateAt { get; set; }
    public DateTime? UpdateAt { get; set; }
    public DateTime? DeleteAt { get; set; }
    public bool IsDeleted { get; set; }
}
