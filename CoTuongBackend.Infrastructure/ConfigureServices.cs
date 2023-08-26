using CoTuongBackend.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace CoTuongBackend.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>();
        return services;
    }
}
