using CoTuongBackend.Application.Matches;
using CoTuongBackend.Application.Rooms;
using CoTuongBackend.Application.Services;
using CoTuongBackend.Application.Users;
using Microsoft.Extensions.DependencyInjection;

namespace CoTuongBackend.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoomService, RoomService>();
        services.AddScoped<IMatchService, MatchService>();

        return services;
    }
}
