using CoTuongBackend.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace CoTuongBackend.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Room>()
            .HasOne(m => m.HostUser)
            .WithMany(u => u.Rooms)
            .HasForeignKey(m => m.HostUserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Match>()
            .HasOne(m => m.HostUser)
            .WithMany(u => u.HostedMatches)
            .HasForeignKey(m => m.HostUserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Match>()
            .HasOne(m => m.OpponentUser)
            .WithMany(u => u.OpponentMatches)
            .HasForeignKey(m => m.OpponentUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
