namespace CoTuongBackend.Domain.Entities;

public class Room : BaseEntity<Guid>
{
    public required string Name { get; set; }
    public int CountUser { get; set; }
    public string? Password { get; set; }
    public Guid HostUserId { get; set; }
    public virtual required ApplicationUser HostUser { get; set; }
    public virtual ICollection<ApplicationUser> Users { get; set; } = new HashSet<ApplicationUser>();
    public virtual ICollection<Match> Matches { get; set; } = new HashSet<Match>();
}
