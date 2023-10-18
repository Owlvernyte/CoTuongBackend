using CoTuongBackend.Domain.Helpers.Games;

namespace CoTuongBackend.Domain.Entities;

public class Room : BaseEntity
{
    public string Code { get; set; } = RoomCodeGenerator.Generate();
    public int CountUser => (HostUserId == Guid.Empty ? 0 : 1) + (OpponentUserId == Guid.Empty ? 0 : 1);
    public string? Password { get; set; }
    public Guid HostUserId { get; set; }
    public virtual ApplicationUser? HostUser { get; set; }
    public Guid? OpponentUserId { get; set; }
    public virtual ApplicationUser? OpponentUser { get; set; }
}
