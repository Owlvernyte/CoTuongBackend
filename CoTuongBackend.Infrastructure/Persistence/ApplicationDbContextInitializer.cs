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
        if (_context.Users.Any()
            || _context.Matches.Any()) return;

        var user = new ApplicationUser
        {
            UserName = "user",
            Email = "user@gmail.com"
        };

        var mei = new ApplicationUser
        {
            UserName = "mei",
            Email = "mei@gmail.com",
            Role = Role.Admin
        };
        var thai = new ApplicationUser
        {
            UserName = "thai",
            Email = "thai@gmail.com",
            Role = Role.Admin
        };
        var tyui = new ApplicationUser
        {
            UserName = "tyui",
            Email = "tyui@gmail.com",
            Role = Role.Admin
        };
        var fiezt = new ApplicationUser
        {
            UserName = "fiezt",
            Email = "fiezt@gmail.com",
            Role = Role.Admin
        };
        var schjr = new ApplicationUser
        {
            UserName = "schjr",
            Email = "schjr@gmail.com",
            Role = Role.Admin
        };
        var meelow = new ApplicationUser
        {
            UserName = "meelow",
            Email = "meelow@gmail.com",
            Role = Role.Admin
        };
        var shiro = new ApplicationUser
        {
            UserName = "shiro",
            Email = "shiro@gmail.com",
            Role = Role.Admin
        };
        var duong = new ApplicationUser
        {
            UserName = "duong",
            Email = "duong@gmail.com",
            Role = Role.Admin
        };

        await _userManager.CreateAsync(user, "P@ssw0rd");

        await _userManager.CreateAsync(mei, "P@ssw0rd");
        await _userManager.CreateAsync(thai, "P@ssw0rd");
        await _userManager.CreateAsync(tyui, "P@ssw0rd");
        await _userManager.CreateAsync(fiezt, "P@ssw0rd");
        await _userManager.CreateAsync(schjr, "P@ssw0rd");
        await _userManager.CreateAsync(meelow, "P@ssw0rd");
        await _userManager.CreateAsync(shiro, "P@ssw0rd");
        await _userManager.CreateAsync(duong, "P@ssw0rd");

        var matches = new List<Match>
        {
            new Match
            {
                UserMatches = new List<UserMatch>
                {
                    new UserMatch
                    {
                        User = thai,
                        Result = MatchResult.Lose
                    },
                    new UserMatch
                    {
                        User = mei,
                        Result = MatchResult.Win
                    }
                }
            },
            new Match
            {
                UserMatches = new List<UserMatch>
                {
                    new UserMatch
                    {
                        User = tyui,
                        Result = MatchResult.Draw
                    },
                    new UserMatch
                    {
                        User = schjr,
                        Result = MatchResult.Draw
                    }
                }
            },
        };

        await _context.AddRangeAsync(matches);

        await _context.SaveChangesAsync().ConfigureAwait(false);
    }
}
