namespace CoTuongBackend.Application.Rooms.Dtos;

public sealed record JoinRoomDto
{
    public string RoomCode { get; set; } = string.Empty;
    public Guid UserId { get; set; }
}
