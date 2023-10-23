using CoTuongBackend.Application.Rooms;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CoTuongBackend.Infrastructure.Services;

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
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var roomCodeList = memoryCache.Get("Delete") as List<string> ?? new List<string>();

                foreach (var roomCode in roomCodeList)
                {
                    await roomService.DeleteWithoutPermission(roomCode);
                }

                memoryCache.Remove("Delete");

                // Wait for a specific interval before running the task again
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
            catch
            {
                throw;
            }
        }
    }
}
