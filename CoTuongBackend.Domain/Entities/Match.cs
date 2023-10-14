using CoTuongBackend.Domain.Enums;

namespace CoTuongBackend.Domain.Entities;

public class Match : BaseEntity
{

    public MatchStatus Status { get; set; } = MatchStatus.Stop;
    public DateTime MatchDate { get; set; } = DateTime.UtcNow;
    public Guid RoomId { get; set; }
    public virtual required Room Room { get; set; }
    public required Guid HostUserId { get; set; }
    public virtual required ApplicationUser HostUser { get; set; }
    public Guid? OpponentUserId { get; set; }
    public virtual ApplicationUser? OpponentUser { get; set; }
}
