using CoTuongBackend.Domain.Interfaces;

namespace CoTuongBackend.Domain.Entities;

public abstract class BaseEntity<TKey> : IAuditableEntity<TKey>
{
    public required TKey Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
