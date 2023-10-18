namespace CoTuongBackend.Application.Matches.Dtos;

public sealed record CreateMatchWithRoomDto(Guid RoomId, Guid? WinnerId);
