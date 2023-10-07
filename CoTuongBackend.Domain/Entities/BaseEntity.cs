using CoTuongBackend.Domain.Interfaces;

namespace CoTuongBackend.Domain.Entities;

public abstract class BaseEntity<TKey> : IAuditableEntity<TKey>
{
    public abstract TKey Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public abstract class BaseEntity : BaseEntity
{
    public override Guid Id { get; set; } = Guid.NewGuid();
}