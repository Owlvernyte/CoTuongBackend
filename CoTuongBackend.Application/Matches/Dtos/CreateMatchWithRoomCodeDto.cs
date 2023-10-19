namespace CoTuongBackend.Application.Matches.Dtos;

public sealed record CreateMatchWithRoomCodeDto(string RoomCode, Guid? WinnerId);
