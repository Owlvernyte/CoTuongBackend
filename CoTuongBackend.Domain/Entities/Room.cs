using CoTuongBackend.Domain.Helpers.Games;

namespace CoTuongBackend.Domain.Entities;

public class Room : BaseEntity
{
    public string Code { get; set; } = RoomCodeGenerator.Generate();
    public int CountUser { get; set; }
    public string? Password { get; set; }
    public Guid HostUserId { get; set; }
    public virtual ApplicationUser? HostUser { get; set; }
    public virtual ICollection<RoomUser> RoomUsers { get; set; } = new HashSet<RoomUser>();
    public virtual ICollection<Match> Matches { get; set; } = new HashSet<Match>();
}
