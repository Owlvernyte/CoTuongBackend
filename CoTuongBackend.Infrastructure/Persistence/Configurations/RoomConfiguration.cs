using CoTuongBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoTuongBackend.Infrastructure.Persistence.Configurations;

public sealed class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder
            .Ignore(x => x.CountUser);

        builder
            .HasOne(x => x.HostUser)
            .WithMany(x => x.HostRoom)
            .HasForeignKey(x => x.HostUserId);

        builder
            .HasOne(x => x.OpponentUser)
            .WithMany(x => x.OpponentRoom)
            .HasForeignKey(x => x.OpponentUserId);
    }
}
