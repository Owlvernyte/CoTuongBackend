using CoTuongBackend.API.Hubs;
using CoTuongBackend.Application.Rooms;
using CoTuongBackend.Infrastructure.Constants;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;

namespace CoTuongBackend.API.BackgroundServices;

public class RoomBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public RoomBackgroundService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var roomService = scope.ServiceProvider.GetRequiredService<IRoomService>();
        var memoryCache = scope.ServiceProvider.GetRequiredService<IMemoryCache>();
        var gameHubContext = scope.ServiceProvider.GetRequiredService<IHubContext<GameHub, IGameHubClient>>();
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var roomCodeList = memoryCache.Get(MemoryCacheConstants.DeleteRoomCodeList) as List<string>;

                if (roomCodeList is { })
                {
                    foreach (var roomCode in roomCodeList)
                    {
                        await roomService.DeleteWithoutPermission(roomCode);
                        await gameHubContext.Clients.Group(roomCode).RoomDeleted().ConfigureAwait(false);
                    }

                    memoryCache.Remove(MemoryCacheConstants.DeleteRoomCodeList);
                }


                // Wait for a specific interval before running the task again
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
            catch
            {
                throw;
            }
        }
    }
}
