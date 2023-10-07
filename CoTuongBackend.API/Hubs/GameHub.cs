using Microsoft.AspNetCore.SignalR;

namespace CoTuongBackend.API.Hubs;

public class GameHub : Hub<IGameHubClient>
{
    public override Task OnConnectedAsync()
    {
        Console.WriteLine("Nguoi choi " + Context.ConnectionId + " da ket noi vao hub");

        Clients.All.Joined("Nguoi choi " + Context.ConnectionId + " da tham gia phong!");

        return base.OnConnectedAsync();
    }
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        Console.WriteLine("Nguoi choi " + Context.ConnectionId + " da ngat ket noi");

        Clients.All.Left("Nguoi choi " + Context.ConnectionId + " da roi phong!");

        return base.OnDisconnectedAsync(exception);
    }
    public Task Move(string source, string destination)
    {
        // connection.invoke("Move", "a1", "a2");

        Console.WriteLine(source + " " + destination);

        Clients.All.Moved("Nguoi choi da di chuyen tu " + source + " den " + destination);

        return Task.CompletedTask;
    }
}
