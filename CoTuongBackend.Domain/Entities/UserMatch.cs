using CoTuongBackend.Domain.Enums;

namespace CoTuongBackend.Domain.Entities;

public class UserMatch
{
    public Guid UserId { get; set; }
    public virtual ApplicationUser? User { get; set; }
    public Guid MatchId { get; set; }
    public virtual Match? Match { get; set; }
    public MatchResult Result { get; set; } = MatchResult.Win;
}
