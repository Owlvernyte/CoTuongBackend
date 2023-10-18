using CoTuongBackend.Application.Rooms.Dtos;
using CoTuongBackend.Application.Users;
using CoTuongBackend.Domain.Entities;
using CoTuongBackend.Domain.Exceptions;
using CoTuongBackend.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using System.Linq.Expressions;

namespace CoTuongBackend.Application.Rooms;

public sealed class RoomService : IRoomService
{
    private readonly ApplicationDbContext _applicationDbContext;

    public RoomService(ApplicationDbContext applicationDbContext)
        => _applicationDbContext = applicationDbContext;

    public async Task<ImmutableList<RoomDto>> Get()
    {
        var roomDtos = await _applicationDbContext.Rooms
            .Include(x => x.HostUser)
            .Include(x => x.OpponentUser)
            .Where(x => x.HostUserId != Guid.Empty
                && x.HostUser != null)
            .Select(room => new RoomDto(
                room.Id,
                room.Code,
                room.CountUser,
                room.Password,
                new UserDto(room.HostUser!.Id, room.HostUser.UserName, room.HostUser.Email),
                room.OpponentUser != null ? new UserDto(room.OpponentUser!.Id, room.OpponentUser.UserName, room.OpponentUser.Email) : null)
            ).ToListAsync();

        return roomDtos.ToImmutableList();
    }

    public async Task<RoomDto> Get(Guid id)
    {
        var room = await _applicationDbContext.Rooms
            .Include(x => x.HostUser)
            .Include(x => x.OpponentUser)
            .SingleOrDefaultAsync(x => x.Id == id)
            ?? throw new NotFoundException(typeof(Room).Name, id);
        if (room.HostUser is null)
        {
            throw new InvalidOperationException("Host not exist!");
        }

        var opponentRoomUser = room.OpponentUser;

        var opponentUser = opponentRoomUser is { }
            ? new UserDto(opponentRoomUser.Id, opponentRoomUser.UserName, opponentRoomUser.Email)
            : null;

        var roomDto = new RoomDto(
            room.Id,
            room.Code,
            room.CountUser,
            room.Password,
            new UserDto(room.HostUser.Id, room.HostUser.UserName, room.HostUser.Email),
            opponentUser);

        return roomDto;
    }

    public async Task<RoomDto> Get(string code)
    {
        var room = await _applicationDbContext.Rooms
            .Include(x => x.HostUser)
            .Include(x => x.OpponentUser)
            .SingleOrDefaultAsync(x => x.Code == code)
            ?? throw new NotFoundException(typeof(Room).Name, code);
        if (room.HostUser is null)
        {
            throw new InvalidOperationException("Host not exist!");
        }

        var opponentRoomUser = room.OpponentUser;

        var opponentUser = opponentRoomUser is { }
            ? new UserDto(opponentRoomUser.Id, opponentRoomUser.UserName, opponentRoomUser.Email)
            : null;

        var roomDto = new RoomDto(
            room.Id,
            room.Code,
            room.CountUser,
            room.Password,
            new UserDto(room.HostUser.Id, room.HostUser.UserName, room.HostUser.Email),
            opponentUser);

        return roomDto;
    }
    public async Task<Guid> Create(CreateRoomDto createRoomDto)
    {
        var room = new Room
        {
            HostUserId = createRoomDto.HostUserId,
            OpponentUserId = createRoomDto.OpponentId,
            Password = createRoomDto.Password,
        };

        await _applicationDbContext
            .AddAsync(room);

        await _applicationDbContext.SaveChangesAsync();

        return room.Id;
    }

    public async Task Join(JoinRoomDto joinRoomDto)
    {
        var room = await _applicationDbContext.Rooms
            .SingleOrDefaultAsync(x => x.Code == joinRoomDto.RoomCode)
            ?? throw new NotFoundException(typeof(Room).Name, joinRoomDto.RoomCode);

        if (room.OpponentUserId is { }
            && room.HostUser is { })
        {
            return;
        }

        if (room.HostUserId == joinRoomDto.UserId
            || room.OpponentUserId == joinRoomDto.UserId)
        {
            return;
        }

        var user = await _applicationDbContext.Users
            .SingleOrDefaultAsync(x => x.Id == joinRoomDto.UserId)
            ?? throw new NotFoundException(typeof(ApplicationUser).Name, joinRoomDto.UserId);

        room.OpponentUser = user;

        await _applicationDbContext.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task<bool> IsExists(Expression<Func<Room, bool>> predicate)
        => await _applicationDbContext.Rooms
        .AnyAsync(predicate);

    public async Task Leave(LeaveRoomDto leaveRoomDto)
    {
        var room = await _applicationDbContext.Rooms
            .Include(x => x.OpponentUser)
            .SingleOrDefaultAsync(x => x.Code == leaveRoomDto.RoomCode)
            ?? throw new NotFoundException(typeof(Room).Name, leaveRoomDto.RoomCode);

        if (room.HostUserId == leaveRoomDto.UserId || room.OpponentUserId is null) return;

        var hasUser = room.OpponentUserId == leaveRoomDto.UserId;

        if (!hasUser) return;

        room.OpponentUserId = null;
        room.OpponentUser = null;

        await _applicationDbContext.SaveChangesAsync().ConfigureAwait(false);
    }
}
