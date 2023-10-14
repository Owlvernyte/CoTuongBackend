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
        var roomDtos = (await _applicationDbContext.Rooms
            .Include(x => x.HostUser)
            .Include(x => x.RoomUsers)
            .ThenInclude(x => x.User)
            .Where(x => x.HostUserId != Guid.Empty
                && x.HostUser != null)
            .ToListAsync())
            .Select(room => new RoomDto(
                room.Id,
                room.Code,
                room.CountUser,
                room.Password,
                new UserDto(room.HostUser!.Id, room.HostUser.UserName, room.HostUser.Email),
                room.RoomUsers.Any(x => x.IsPlayer && x.User != null)
                    ? new UserDto(
                        room.RoomUsers.FirstOrDefault(x => x.IsPlayer && x.User != null)!.User!.Id,
                        room.RoomUsers.FirstOrDefault(x => x.IsPlayer && x.User != null)!.User!.UserName,
                        room.RoomUsers.FirstOrDefault(x => x.IsPlayer && x.User != null)!.User!.Email)
                    : null)
            ).ToList();

        return roomDtos.ToImmutableList();
    }

    public async Task<RoomDto> Get(Guid id)
    {
        var room = await _applicationDbContext.Rooms
            .Include(x => x.HostUser)
            .Include(x => x.RoomUsers)
            .ThenInclude(x => x.User)
            .SingleOrDefaultAsync(x => x.Id == id)
            ?? throw new NotFoundException(typeof(Room).Name, id);
        if (room.HostUser is null)
        {
            throw new InvalidOperationException("Host not exist!");
        }

        var opponentRoomUser = room.RoomUsers.FirstOrDefault(x => x.IsPlayer);

        var opponentUser = opponentRoomUser is { User: { } }
            ? new UserDto(opponentRoomUser.UserId, opponentRoomUser.User.UserName, opponentRoomUser.User.Email)
            : null;

        var roomDto = new RoomDto(
            room.Id,
            room.Code,
            room.CountUser,
            room.Password,
            new UserDto(room.HostUser.Id, room.HostUser.UserName, room.HostUser.Email),
            opponentUser)
        {
            Users = room.RoomUsers.Select(x => new UserDto(x.UserId, x.User?.UserName, x.User?.Email))
        };

        return roomDto;
    }
    public async Task<Guid> Create(CreateRoomDto createRoomDto)
    {
        var room = new Room
        {
            HostUserId = createRoomDto.HostUserId,
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
            .Include(x => x.RoomUsers)
            .SingleOrDefaultAsync(x => x.Code == joinRoomDto.RoomCode)
            ?? throw new NotFoundException(typeof(Room).Name, joinRoomDto.RoomCode);

        if (room.HostUserId == joinRoomDto.UserId
            || room.RoomUsers.Any(x => x.UserId == joinRoomDto.UserId))
        {
            return;
        }

        var user = await _applicationDbContext.Users
            .SingleOrDefaultAsync(x => x.Id == joinRoomDto.UserId)
            ?? throw new NotFoundException(typeof(ApplicationUser).Name, joinRoomDto.UserId);

        room.CountUser += 1;

        room.RoomUsers.Add(new RoomUser { User = user, IsPlayer = !room.RoomUsers.Any() });

        await _applicationDbContext.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task<bool> IsExists(Expression<Func<Room, bool>> predicate)
        => await _applicationDbContext.Rooms
        .AnyAsync(predicate);

    public async Task Leave(LeaveRoomDto leaveRoomDto)
    {
        var room = await _applicationDbContext.Rooms
            .Include(x => x.RoomUsers)
            .SingleOrDefaultAsync(x => x.Code == leaveRoomDto.RoomCode)
            ?? throw new NotFoundException(typeof(Room).Name, leaveRoomDto.RoomCode);

        if (room.HostUserId == leaveRoomDto.UserId) return;

        var user = room.RoomUsers.FirstOrDefault(x => x.UserId == leaveRoomDto.UserId);

        if (user is null) return;

        room.CountUser -= 1;

        room.RoomUsers.Remove(user);

        await _applicationDbContext.SaveChangesAsync().ConfigureAwait(false);
    }
}
