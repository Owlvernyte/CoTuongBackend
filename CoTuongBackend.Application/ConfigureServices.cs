using CoTuongBackend.Application.Matches;
using CoTuongBackend.Application.Rooms;
using CoTuongBackend.Application.Services;
using CoTuongBackend.Application.Users;
using CoTuongBackend.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CoTuongBackend.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoomService, RoomService>();
        services.AddScoped<IMatchService, MatchService>();

        services.AddHostedService<RoomBackgroundService>();
        return services;
    }
}
