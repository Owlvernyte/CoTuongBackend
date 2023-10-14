using CoTuongBackend.Application.Chat.Dtos;
using CoTuongBackend.Application.Users;
using CoTuongBackend.Domain.Services;
using Microsoft.AspNetCore.SignalR;
using SignalRSwaggerGen.Attributes;

namespace CoTuongBackend.API.Hubs;

[SignalRHub]
public class ChatHub : Hub<IChatHubClient>
{
    private readonly IUserAccessor _userAccessor;

    public ChatHub(IUserAccessor userAccessor)
    {
        _userAccessor = userAccessor;
    }
    public override Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();
        if (httpContext == null)
            return base.OnConnectedAsync();
        if (!httpContext.Request.Query
            .TryGetValue("roomCode", out var roomCodeStringValues))
            return base.OnConnectedAsync();

        var roomCode = roomCodeStringValues.ToString();
        //Console.WriteLine("Nguoi choi " + Context.ConnectionId + " da ket noi vao hub " + roomCode);
        Groups.AddToGroupAsync(Context.ConnectionId, roomCode);
        Clients.Group(roomCode).Joined(new UserDto(_userAccessor.Id, _userAccessor.UserName, _userAccessor.Email));

        return base.OnConnectedAsync();
    }
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        // Room Id
        var httpContext = Context.GetHttpContext();
        if (httpContext == null)
            return base.OnDisconnectedAsync(exception);
        if (!httpContext.Request.Query
            .TryGetValue("roomCode", out var roomCodeStringValues))
            return base.OnDisconnectedAsync(exception);

        var roomCode = roomCodeStringValues.ToString();

        Groups.RemoveFromGroupAsync(Context.ConnectionId, roomCode);

        Clients.Group(roomCode).Left(new UserDto(_userAccessor.Id, _userAccessor.UserName, _userAccessor.Email));

        Console.WriteLine("Nguoi choi " + Context.ConnectionId + " da ngat ket noi");

        return base.OnDisconnectedAsync(exception);
    }
    public Task Chat(ChatMessageDto chatMessageDto)
    {
        var (roomCode, message) = chatMessageDto;
        Console.WriteLine("Nguoi choi " + Context.ConnectionId + " da chat " + message + " vao hub " + roomCode);

        Clients.Group(roomCode).Chat(message, roomCode, new UserDto(_userAccessor.Id, _userAccessor.UserName, _userAccessor.Email));

        return Task.CompletedTask;
    }
}
