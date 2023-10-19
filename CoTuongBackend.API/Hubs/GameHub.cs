using CoTuongBackend.Application.Games.Dtos;
using CoTuongBackend.Application.Matches;
using CoTuongBackend.Application.Matches.Dtos;
using CoTuongBackend.Application.Rooms;
using CoTuongBackend.Application.Rooms.Dtos;
using CoTuongBackend.Application.Users;
using CoTuongBackend.Domain.Entities.Games;
using CoTuongBackend.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SignalRSwaggerGen.Attributes;

namespace CoTuongBackend.API.Hubs;

[SignalRHub]
[Authorize]
public sealed class GameHub : Hub<IGameHubClient>
{
    private readonly IRoomService _roomService;
    private readonly IMatchService _matchService;
    private readonly IUserAccessor _userAccessor;
    private readonly ILogger<GameHub> _logger;

    public GameHub(IRoomService roomService, IMatchService matchService, IUserAccessor userAccessor, ILogger<GameHub> logger)
    {
        _roomService = roomService;
        _matchService = matchService;
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
        {
            Context.Abort();
        }

        var roomCode = roomCodeStringValues.ToString();

        var isExists = await _roomService.IsExists(x => x.Code == roomCode);

        if (!isExists)
        {
            Context.Abort();
            return;
        }

        await _roomService.Join(new JoinRoomDto(roomCode, _userAccessor.Id));

        if (!await _roomService.IsExists(x => x.OpponentUserId == _userAccessor.Id || x.HostUserId == _userAccessor.Id))
        {
            Context.Abort();
            return;
        }

        // Check Room in Boards
        var hasRoom = Boards.TryGetValue(roomCode, out var board);

        if (!hasRoom)
        {
            Boards.Add(roomCode, new Board());

            board = Boards[roomCode];
        }

        if (board is null)
        {
            Context.Abort();
            return;
        }

        // Send Board info to group
        _logger.LogInformation("Nguoi choi {UserName} - {ConnectionId} da ket noi vao hub", _userAccessor.UserName, Context.ConnectionId);

        await Groups.AddToGroupAsync(Context.ConnectionId, roomCode);

        await Clients.Client(Context.ConnectionId).LoadBoard(board.Squares);

        await Clients.Group(roomCode)
            .Joined(new UserDto(_userAccessor.Id, _userAccessor.UserName, _userAccessor.Email));

        return;
    }
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogInformation("Nguoi choi {UserName} - {ConnectionId} da ngat ket noi", _userAccessor.UserName, Context.ConnectionId);

        // Get Room Code
        var httpContext = Context.GetHttpContext();
        if (httpContext == null)
        {
            Context.Abort();
            return;
        }
        if (!httpContext.Request.Query
            .TryGetValue("roomCode", out var roomCodeStringValues))
        {
            Context.Abort();
            return;
        }

        var roomCode = roomCodeStringValues.ToString();

        //await _roomService.Leave(new LeaveRoomDto(roomCode, _userAccessor.Id));

        // Remove the user out the group
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomCode);

        await Clients.Group(roomCode)
            .Left(new UserDto(_userAccessor.Id, _userAccessor.UserName, _userAccessor.Email));
    }
    public async Task NewGame()
    {

        var httpContext = Context.GetHttpContext();
        if (httpContext == null)
            return;
        if (!httpContext.Request.Query
            .TryGetValue("roomCode", out var roomCodeStringValues))
            return;

        var roomCode = roomCodeStringValues.ToString();

        var isExists = await _roomService.IsExists(x => x.Code == roomCode && x.HostUserId == _userAccessor.Id);

        if (!isExists)
        {
            return;
        }
        var hasBoard = Boards.TryGetValue(roomCode, out var board);

        if (hasBoard)
        {
            Boards[roomCode].Reset();
        }
        else
        {
            Boards.Add(roomCode, new Board());
        }

        board = Boards[roomCode];

        await Clients.Group(roomCode).LoadBoard(board.Squares);

        return;
    }
    public async Task Move(MovePieceDto movePieceDto)
    {
        var httpContext = Context.GetHttpContext();
        if (httpContext == null)
            return;
        if (!httpContext.Request.Query
            .TryGetValue("roomCode", out var roomCodeStringValues))
            return;

        var roomCode = roomCodeStringValues.ToString();

        var (source, destination) = movePieceDto;

        var hasRoom = Boards.TryGetValue(roomCode, out var board);

        if (!hasRoom)
        {
            await Clients.Client(Context.ConnectionId).MoveFailed(source, destination);
            return;
        }

        if (board is null)
        {
            await Clients.Client(Context.ConnectionId).MoveFailed(source, destination);
            return;
        }

        var piece = board.GetPiece(source);

        if (piece is null)
        {
            await Clients.Client(Context.ConnectionId).MoveFailed(source, destination);
            return;
        }

        var isValid = board.Move(piece, destination);

        if (!isValid)
        {
            await Clients.Client(Context.ConnectionId).MoveFailed(source, destination);
            return;
        }

        await Clients.Group(roomCode).Moved(source, destination, !piece.IsRed);

        if (board.IsOpponentGeneral(piece, destination))
        {
            await _matchService.Create(new CreateMatchWithRoomCodeDto(roomCode, _userAccessor.Id));
            await Clients.Group(roomCode).Ended(piece.IsRed, new UserDto(_userAccessor.Id, _userAccessor.UserName, _userAccessor.Email));
        }

        return;
    }
    public Task Chat(string message)
    {
        var httpContext = Context.GetHttpContext();
        if (httpContext == null)
            return Task.CompletedTask;
        if (!httpContext.Request.Query
            .TryGetValue("roomCode", out var roomCodeStringValues))
            return Task.CompletedTask;

        var roomCode = roomCodeStringValues.ToString();

        _logger.LogInformation("Nguoi choi {UserName} - {ConnectionId} da chat {Message} vao hub {RoomCode}", _userAccessor.UserName, Context.ConnectionId, message, roomCode);

        Clients.Group(roomCode).Chatted(message, roomCode, new UserDto(_userAccessor.Id, _userAccessor.UserName, _userAccessor.Email));
        return Task.CompletedTask;
    }
}
