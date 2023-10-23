using CoTuongBackend.Domain.Enums;

namespace CoTuongBackend.Domain.Entities;

public class Match : BaseEntity
{
    public MatchStatus Status { get; set; } = MatchStatus.Stop;
    public DateTime MatchDate { get; set; } = DateTime.UtcNow;
    public virtual ICollection<UserMatch> UserMatches { get; set; } = new HashSet<UserMatch>();
}
