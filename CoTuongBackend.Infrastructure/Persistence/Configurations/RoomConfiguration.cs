using CoTuongBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoTuongBackend.Infrastructure.Persistence.Configurations;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder
            .Property(r => r.CountUser)
            .HasDefaultValue(1);

        builder
            .HasOne(m => m.HostUser)
            .WithMany(u => u.Rooms)
            .HasForeignKey(m => m.HostUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
