using CoTuongBackend.Application.Users;
using CoTuongBackend.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CoTuongBackend.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        return services;
    }
}
