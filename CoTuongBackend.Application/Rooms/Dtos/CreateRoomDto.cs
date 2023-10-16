namespace CoTuongBackend.Application.Rooms.Dtos;

public sealed record CreateRoomDto(string? Password, Guid HostUserId, Guid? OpponentId);