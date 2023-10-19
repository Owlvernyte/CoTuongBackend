namespace CoTuongBackend.Application.Matches.Dtos;

public sealed record CreateMatchWithRoomIdDto(Guid RoomId, Guid? WinnerId);
