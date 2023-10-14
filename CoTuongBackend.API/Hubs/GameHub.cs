using CoTuongBackend.Application.Chat.Dtos;
using CoTuongBackend.Application.Games.Dtos;
using CoTuongBackend.Application.Rooms;
using CoTuongBackend.Application.Users;
using CoTuongBackend.Domain.Entities.Games;
using CoTuongBackend.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SignalRSwaggerGen.Attributes;

namespace CoTuongBackend.API.Hubs;

[SignalRHub]
[Authorize]
public class GameHub : Hub<IGameHubClient>
{
    private readonly IRoomService _roomService;
    private readonly IUserAccessor _userAccessor;
    private readonly ILogger<GameHub> _logger;

    public GameHub(IRoomService roomService, IUserAccessor userAccessor, ILogger<GameHub> logger)
    {
        _roomService = roomService;
        _userAccessor = userAccessor;
        _logger = logger;
    }
    public static Dictionary<string, Board> Boards { get; set; } = new();
    public override async Task OnConnectedAsync()
    {
        // Get Room Code
        var httpContext = Context.GetHttpContext();
        if (httpContext == null)
            return;
        if (!httpContext.Request.Query
            .TryGetValue("roomCode", out var roomCodeStringValues))
            return;

        var roomCode = roomCodeStringValues.ToString();

        var isExists = await _roomService.IsExists(x => x.Code == roomCode);

        // Check Room in Boards
        var hasRoom = Boards.TryGetValue(roomCode, out var board);

        if (!hasRoom)
        {
            Boards.Add(roomCode, new Board());

            board = Boards[roomCode];
        }

        if (board is null) return;

        // Send Board info to group
        _logger.LogInformation("Nguoi choi {UserName} - {ConnectionId} da ket noi vao hub", _userAccessor.UserName, Context.ConnectionId);

        await Groups.AddToGroupAsync(Context.ConnectionId, roomCode);

        await Clients.Client(Context.ConnectionId).LoadBoard(board.Squares);

        await Clients.Group(roomCode)
            .Joined(new UserDto(_userAccessor.Id, _userAccessor.UserName, _userAccessor.Email));

        return;
    }
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogInformation("Nguoi choi {UserName} - {ConnectionId} da ngat ket noi", _userAccessor.UserName, Context.ConnectionId);

        // Get Room Code
        var httpContext = Context.GetHttpContext();
        if (httpContext == null)
            return base.OnDisconnectedAsync(exception);
        if (!httpContext.Request.Query
            .TryGetValue("roomCode", out var roomCodeStringValues))
            return base.OnDisconnectedAsync(exception);

        var roomCode = roomCodeStringValues.ToString();

        // Remove the user out the group
        Groups.RemoveFromGroupAsync(Context.ConnectionId, roomCode);

        Clients.Group(roomCode)
            .Left(new UserDto(_userAccessor.Id, _userAccessor.UserName, _userAccessor.Email));

        return base.OnDisconnectedAsync(exception);
    }
    public Task Move(MovePieceDto movePieceDto)
    {
        var (roomCode, source, destination) = movePieceDto;

        var hasRoom = Boards.TryGetValue(roomCode, out var board);

        if (!hasRoom)
        {
            Clients.Client(Context.ConnectionId).MoveFailed(source, destination);
            return Task.CompletedTask;
        }

        if (board is null)
        {
            Clients.Client(Context.ConnectionId).MoveFailed(source, destination);
            return Task.CompletedTask;
        }

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

        Clients.Group(roomCode).Moved(source, destination, !piece.IsRed);

        return Task.CompletedTask;
    }
    public Task Chat(ChatMessageDto chatMessageDto)
    {
        var (roomCode, message) = chatMessageDto;
        _logger.LogInformation("Nguoi choi {UserName} - {ConnectionId} da chat {Message} vao hub {RoomCode}", _userAccessor.UserName, Context.ConnectionId, message, roomCode);

        Clients.Group(roomCode).Chat(message, roomCode, new UserDto(_userAccessor.Id, _userAccessor.UserName, _userAccessor.Email));
        return Task.CompletedTask;
    }
}
