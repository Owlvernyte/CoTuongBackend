using CoTuongBackend.Domain.Entities.Games;
using Microsoft.AspNetCore.SignalR;

namespace CoTuongBackend.API.Hubs;

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
            return Task.CompletedTask;
        if (!httpContext.Request.Query
            .TryGetValue("roomId", out var roomId))
            return Task.CompletedTask;

        var board = Boards[roomId.ToString()];


        Console.WriteLine("Nguoi choi " + Context.ConnectionId + " da ket noi vao hub");

        Clients.All.Joined(board.GetPieceMatrix());

        return base.OnConnectedAsync();
    }
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        Console.WriteLine("Nguoi choi " + Context.ConnectionId + " da ngat ket noi");

        Clients.All.Left("Nguoi choi " + Context.ConnectionId + " da roi phong!");

        return base.OnDisconnectedAsync(exception);
    }
    public Task Move(Coordinate source, Coordinate destination)
    {
        var roomId = "RoomId";

        var board = Boards[roomId];

        var piece = board.GetPiece(source);

        if (piece is null) return Task.CompletedTask;

        var isValid = board.Move(piece, destination);

        if (!isValid) return Task.CompletedTask;

        Console.WriteLine(source + " " + destination);

        Clients.All.Moved(source, destination, piece.IsRed);

        return Task.CompletedTask;
    }
}
