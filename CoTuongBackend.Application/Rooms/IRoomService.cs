using CoTuongBackend.Application.Rooms.Dtos;
using CoTuongBackend.Domain.Entities;
using System.Collections.Immutable;
using System.Linq.Expressions;

namespace CoTuongBackend.Application.Rooms;
public interface IRoomService
{
    Task<Guid> Create(CreateRoomDto createRoomDto);
    Task<ImmutableList<RoomDto>> Get();
    Task Join(JoinRoomDto joinRoomDto);
    Task Leave(LeaveRoomDto leaveRoomDto);
    Task<RoomDto> Get(Guid id);
    Task<bool> IsExists(Expression<Func<Room, bool>> predicate);
}