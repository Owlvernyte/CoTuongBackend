namespace CoTuongBackend.Domain.Entities;

public class RoomUser
{
    public Guid UserId { get; set; }
    public virtual ApplicationUser? User { get; set; }
    public Guid RoomId { get; set; }
    public virtual Room? Room { get; set; }
    public bool IsPlayer { get; set; } = false;
}
