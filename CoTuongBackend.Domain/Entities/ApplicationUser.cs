using CoTuongBackend.Domain.Enums;
using CoTuongBackend.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CoTuongBackend.Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>, IAuditableEntity<Guid>
{
    [PersonalData]
    public Role Role { get; set; } = Role.User;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual ICollection<Room> Rooms { get; set; } = new HashSet<Room>();
    public virtual ICollection<Match> HostedMatches { get; set; } = new HashSet<Match>();
    public virtual ICollection<Match> OpponentMatches { get; set; } = new HashSet<Match>();
}
