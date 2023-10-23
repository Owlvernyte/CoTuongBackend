using CoTuongBackend.Application.Games.Dtos;
using CoTuongBackend.Application.Games.Enums;
using CoTuongBackend.Application.Matches;
using CoTuongBackend.Application.Matches.Dtos;
using CoTuongBackend.Application.Rooms;
using CoTuongBackend.Application.Rooms.Dtos;
using CoTuongBackend.Application.Users;
using CoTuongBackend.Domain.Entities.Games;
using CoTuongBackend.Domain.Exceptions;
using CoTuongBackend.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using SignalRSwaggerGen.Attributes;
using System.Timers;
using Timer = System.Timers.Timer;

namespace CoTuongBackend.API.Hubs;

[SignalRHub]
[Authorize]
public sealed class GameHub : Hub<IGameHubClient>
{
    private readonly IRoomService _roomService;
    private readonly IMatchService _matchService;
    private readonly IUserAccessor _userAccessor;
    private readonly ILogger<GameHub> _logger;
    private readonly IMemoryCache _memoryCache;

    public GameHub(IRoomService roomService, IMatchService matchService, IUserAccessor userAccessor, ILogger<GameHub> logger, IMemoryCache memoryCache)
    {
        _roomService = roomService;
        _matchService = matchService;
        _userAccessor = userAccessor;
        _logger = logger;
        _memoryCache = memoryCache;
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

        await Clients.Client(Context.ConnectionId).LoadBoard(board.Squares, board.IsHostRed, board.IsRedTurn);

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
            return;
        }
        if (!httpContext.Request.Query
            .TryGetValue("roomCode", out var roomCodeStringValues))
        {
            return;
        }

        var roomCode = roomCodeStringValues.ToString();

        //await _roomService.Leave(new LeaveRoomDto(roomCode, _userAccessor.Id));

        RoomDto room;

        try
        {
            room = await _roomService.Get(roomCode);
        }
        catch (NotFoundException)
        {
            return;
        }

        if (room is null)
        {
            return;
        }

        if (room.HostUser.Id == _userAccessor.Id)
        {
            var timeoutSecond = 5;
            var timeout = timeoutSecond * 1000;
            var timer = new Timer(timeout)
            {
                AutoReset = false
            };
            await Clients.Group(roomCode).HostLeft(timeoutSecond);
            timer.Elapsed += new ElapsedEventHandler(async (sender, o) =>
            {
                var absoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10);
                var list = await _memoryCache.GetOrCreateAsync(
                    "Delete",
                    x =>
                    {
                        x.AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow;
                        return Task.FromResult(new List<string> { roomCode });
                    });
                if (list is null)
                {
                    _memoryCache.Set("Delete", new List<string> { roomCode }, absoluteExpirationRelativeToNow);
                }
                else
                {
                    list.Add(roomCode);
                    _memoryCache.Set("Delete", list, absoluteExpirationRelativeToNow);
                }
            });
            timer.Enabled = true;
        }

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

        await Clients.Group(roomCode).LoadBoard(board.Squares, board.IsHostRed, board.IsRedTurn);

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

        if (source == destination) return;

        var hasRoom = Boards.TryGetValue(roomCode, out var board);

        if (!hasRoom)
        {
            await Clients.Client(Context.ConnectionId).MoveFailed(MoveStatus.RoomNotFound);
            return;
        }

        if (board is null)
        {
            await Clients.Client(Context.ConnectionId).MoveFailed(MoveStatus.BoardNotFound);
            return;
        }

        var piece = board.GetPiece(source);

        if (piece is null)
        {
            await Clients.Client(Context.ConnectionId).MoveFailed(MoveStatus.PieceNotFound);
            return;
        }
        if (!piece.IsValidMove(destination, board))
        {
            await Clients.Client(Context.ConnectionId).MoveFailed(MoveStatus.InvalidMove);
            return;
        }
        if (board.IsOpponentGeneral(piece, destination))
        {
            await _matchService.Create(new CreateMatchWithRoomCodeDto(roomCode, _userAccessor.Id));
            await Clients.Group(roomCode).Ended(piece.IsRed, new UserDto(_userAccessor.Id, _userAccessor.UserName, _userAccessor.Email));
        }

        board.Move(piece, destination);
        board.IsRedTurn = !piece.IsRed;
        await Clients.Group(roomCode).Moved(source, destination, !piece.IsRed);

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
