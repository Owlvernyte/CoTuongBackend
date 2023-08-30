using CoTuongBackend.Domain.Entities;
using CoTuongBackend.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CoTuongBackend.Infrastructure.Persistence;

public class ApplicationDbContextInitializer
{
    private readonly ILogger<ApplicationDbContextInitializer> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public ApplicationDbContextInitializer(ILogger<ApplicationDbContextInitializer> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
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
            _logger.LogError(ex, "Migration error");
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
            _logger.LogError(ex, "Seeding error");
        }
    }

    public async Task TrySeedAsync()
    {
        if (_context.Users.Any()) return;

        var tyui = new ApplicationUser
        {
            UserName = "tyui",
            Email = "tyui@gmail.com",
            Role = Role.User
        };
        var thai = new ApplicationUser
        {
            UserName = "thai",
            Email = "thai@gmail.com",
            Role = Role.Admin
        };
        await _userManager.CreateAsync(tyui, "12345678");
        await _userManager.CreateAsync(thai, "12345678");
    }
}
