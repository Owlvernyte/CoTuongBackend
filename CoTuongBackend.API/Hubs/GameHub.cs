using CoTuongBackend.Application.Games.Dtos;
using CoTuongBackend.Domain.Entities.Games;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace CoTuongBackend.API.Hubs;

[Authorize]
public class GameHub : Hub<IGameHubClient>
{
    public static Dictionary<string, Board> Boards { get; set; } = new Dictionary<string, Board>
    {
        ["RoomId"] = new Board(),
    };
    public override Task OnConnectedAsync()
    {
        // Room Id
        var httpContext = Context.GetHttpContext();
        if (httpContext == null)
            return base.OnConnectedAsync();
        if (!httpContext.Request.Query
            .TryGetValue("roomId", out var roomIdStringValues))
            return base.OnConnectedAsync();

        var roomId = roomIdStringValues.ToString();

        var board = Boards[roomId];


        Console.WriteLine("Nguoi choi " + Context.ConnectionId + " da ket noi vao hub");

        Groups.AddToGroupAsync(Context.ConnectionId, roomId);

        Clients.Group(roomId).Joined(board.GetPieceMatrix());

        return base.OnConnectedAsync();
    }
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        Console.WriteLine($"Nguoi choi {Context.ConnectionId} da ngat ket noi");

        // Room Id
        var httpContext = Context.GetHttpContext();
        if (httpContext == null)
            return base.OnDisconnectedAsync(exception);
        if (!httpContext.Request.Query
            .TryGetValue("roomId", out var roomIdStringValues))
            return base.OnDisconnectedAsync(exception);

        var roomId = roomIdStringValues.ToString();

        Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);

        Clients.All.Left("Nguoi choi " + Context.ConnectionId + " da roi phong!");

        return base.OnDisconnectedAsync(exception);
    }
    public Task Move(MovePieceDto movePieceDto)
    {
        var (roomId, source, destination) = movePieceDto;

        var board = Boards[roomId];

        var piece = board.GetPiece(source);

        if (piece is null)
        {
            Clients.Client(Context.ConnectionId).MoveFailed(source, destination);
            return Task.CompletedTask;
        }

        var isValid = board.Move(piece, destination);

        if (!isValid)
        {
            Clients.Client(Context.ConnectionId).MoveFailed(source, destination);
            return Task.CompletedTask;
        }

        Clients.Group(roomId).Moved(source, destination, !piece.IsRed);

        return Task.CompletedTask;
    }
}
