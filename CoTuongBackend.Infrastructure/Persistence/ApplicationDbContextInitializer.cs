using CoTuongBackend.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CoTuongBackend.Infrastructure.Persistence;

public class ApplicationDbContextInitializer
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public ApplicationDbContextInitializer(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            await _context.Database.MigrateAsync();

        }
        catch (Exception ex)
        {
            //Log.Error(ex, "Migration error");
        }
    }
    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            //Log.Error(ex, "Seeding error");
        }
    }

    public async Task TrySeedAsync()
    {
        if (_context.Users.Any()) return;

        var tyui = new ApplicationUser
        {
            UserName = "tyui",
            Email = "tyui@gmail.com"
        };
        await _userManager.CreateAsync(tyui, "12345678");
    }
}
