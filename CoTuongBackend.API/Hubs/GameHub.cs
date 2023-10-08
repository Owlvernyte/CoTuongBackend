using CoTuongBackend.Domain.Entities.Games;
using Microsoft.AspNetCore.SignalR;

namespace CoTuongBackend.API.Hubs;

public class GameHub : Hub<IGameHubClient>
{
    public static Dictionary<string, Board> Boards { get; set; } = new Dictionary<string, Board>
    {
        ["RoomId"] = new Board
        {
            Squares = new Piece?[9, 10]
            {
                { null, null, null, null, null, null, null, null, null, null},
                { null, null, null, null, null, null, null, null, null, null},
                { null, null, null, null, null, null, null, null, null, null},
                { null, null, null, null, null, null, null, null, null, null},
                { null, null, null, null, null, null, null, null, null, null},
                { null, null, null, null, null, null, null, null, null, null},
                { null, null, null, null, null, null, null, null, null, null},
                { null, null, null, null, null, null, null, null, null, null},
                { null, null, null, null, null, null, null, null, null, null},
            }
        }
    };
    public override Task OnConnectedAsync()
    {
        // Room Id

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
    public Task Move(Coordinate source, Coordinate destination)
    {
        // connection.invoke("Move", "a1", "a2");

        var roomId = "RoomId";

        Piece piece = Boards[roomId].Squares[source.X, source.Y]!;

        var isValid = piece.IsValidMove(destination, Boards[roomId]);

        // Move
        if (!isValid) return Task.CompletedTask;

        Boards[roomId].Squares[destination.X, destination.Y] = Boards[roomId].Squares[source.X, source.Y];

        Boards[roomId].Squares[source.X, source.Y] = null;

        Console.WriteLine(source + " " + destination);

        Clients.All.Moved(source, destination, piece.IsRed);

        return Task.CompletedTask;
    }
}
