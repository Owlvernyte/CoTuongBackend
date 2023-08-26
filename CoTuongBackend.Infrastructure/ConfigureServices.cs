using CoTuongBackend.Domain.Entities;
using CoTuongBackend.Domain.Interfaces;
using CoTuongBackend.Infrastructure.Persistence;
using CoTuongBackend.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace CoTuongBackend.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddIdentityCore<ApplicationUser>(options =>
        {
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 8;

            options.User.RequireUniqueEmail = true;

            options.SignIn.RequireConfirmedAccount = false;
            options.SignIn.RequireConfirmedEmail = false;
            options.SignIn.RequireConfirmedPhoneNumber = false;

            options.ClaimsIdentity.UserNameClaimType = ClaimTypes.Name;
            options.ClaimsIdentity.UserIdClaimType = ClaimTypes.NameIdentifier;
            options.ClaimsIdentity.EmailClaimType = ClaimTypes.Email;
        })
            //.AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddScoped<ApplicationDbContextInitializer>();

        services.AddScoped<ITokenService, TokenService>();

        return services;
    }
}
