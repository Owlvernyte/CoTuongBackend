namespace CoTuongBackend.Domain.Interfaces;
public interface IAuditableEntity<TKey> : IEntity<TKey>
{
    DateTime CreatedAt { get; set; }
}
