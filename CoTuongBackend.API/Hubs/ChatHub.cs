using CoTuongBackend.Application.Chat.Dtos;
using Microsoft.AspNetCore.SignalR;
using SignalRSwaggerGen.Attributes;

namespace CoTuongBackend.API.Hubs;

[SignalRHub]
public class ChatHub : Hub<IChatHubClient>
{
    public override Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();
        if (httpContext == null)
            return base.OnConnectedAsync();
        if (!httpContext.Request.Query
            .TryGetValue("roomId", out var roomIdStringValues))
            return base.OnConnectedAsync();

        var roomId = roomIdStringValues.ToString();
        Console.WriteLine("Nguoi choi " + Context.ConnectionId + " da ket noi vao hub " + roomId);
        Groups.AddToGroupAsync(Context.ConnectionId, roomId);
        Clients.Group(roomId).Joined("Nguoi choi " + Context.ConnectionId + " da tham gia phong! " + roomId);

        return base.OnConnectedAsync();
    }
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        // Room Id
        var httpContext = Context.GetHttpContext();
        if (httpContext == null)
            return base.OnDisconnectedAsync(exception);
        if (!httpContext.Request.Query
            .TryGetValue("roomId", out var roomIdStringValues))
            return base.OnDisconnectedAsync(exception);

        var roomId = roomIdStringValues.ToString();

        Clients.Group(roomId).Left("Nguoi choi " + Context.ConnectionId + " da roi phong!");

        Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
        Console.WriteLine("Nguoi choi " + Context.ConnectionId + " da ngat ket noi");


        return base.OnDisconnectedAsync(exception);
    }
    public Task Chat(ChatMessageDto chatMessageDto)
    {
        var (roomId, message) = chatMessageDto;
        Console.WriteLine("Nguoi choi " + Context.ConnectionId + " da chat " + message + " vao hub " + roomId);

        Clients.Group(roomId).Chat("Nguoi choi " + Context.ConnectionId + " da chat " + message);

        return Task.CompletedTask;
    }
}
