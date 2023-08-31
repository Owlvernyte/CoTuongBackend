using CoTuongBackend.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoTuongBackend.Domain.Entities
{
    public class Match : BaseEntity<Guid>
    {
        public required Guid HostUserId { get; set; }
        public Guid? OpponentUserId { get; set; }

        public MatchStatus Status { get; set; } = MatchStatus.Stop;
        public DateTime MatchDate { get; set; } = DateTime.UtcNow;
        public virtual required ApplicationUser HostUser { get; set; }
        public virtual ApplicationUser? OpponentUser { get; set; }
    }
}
