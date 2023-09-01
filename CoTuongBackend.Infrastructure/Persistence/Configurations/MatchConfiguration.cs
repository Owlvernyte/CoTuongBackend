using CoTuongBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoTuongBackend.Infrastructure.Persistence.Configurations;

public class MatchConfiguration : IEntityTypeConfiguration<Match>
{
    public void Configure(EntityTypeBuilder<Match> builder)
    {
        builder
            .HasOne(m => m.HostUser)
            .WithMany(u => u.HostedMatches)
            .HasForeignKey(m => m.HostUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(m => m.OpponentUser)
            .WithMany(u => u.OpponentMatches)
            .HasForeignKey(m => m.OpponentUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(m => m.Room)
            .WithMany(u => u.Matches)
            .HasForeignKey(m => m.RoomId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
