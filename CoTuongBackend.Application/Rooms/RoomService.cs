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
            .Where(x => x.HostUserId != Guid.Empty)
            .Where(x => x.HostUser != null)
            .Select(room => new RoomDto(
            room.Id,
            room.Code,
            room.CountUser,
            room.Password,
            new UserDto(room.HostUser!.Id, room.HostUser.UserName, room.HostUser.Email)))
            .ToListAsync();

        return roomDtos.ToImmutableList();
    }

    public async Task<RoomDto> Get(Guid id)
    {
        var room = await _applicationDbContext.Rooms
            .Include(x => x.HostUser)
            .SingleOrDefaultAsync(x => x.Id == id)
            ?? throw new NotFoundException(typeof(Room).Name, id);
        if (room.HostUser is null)
        {
            throw new InvalidOperationException("Host not exist!");
        }

        var roomDto = new RoomDto(
            room.Id,
            room.Code,
            room.CountUser,
            room.Password,
            new UserDto(room.HostUser.Id, room.HostUser.UserName, room.HostUser.Email));

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
}
