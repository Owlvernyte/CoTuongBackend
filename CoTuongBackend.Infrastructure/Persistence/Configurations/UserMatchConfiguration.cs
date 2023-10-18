using CoTuongBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoTuongBackend.Infrastructure.Persistence.Configurations;

public sealed class UserMatchConfiguration : IEntityTypeConfiguration<UserMatch>
{
    public void Configure(EntityTypeBuilder<UserMatch> builder)
    {
        builder
            .HasKey(x => new { x.UserId, x.MatchId });
    }
}
