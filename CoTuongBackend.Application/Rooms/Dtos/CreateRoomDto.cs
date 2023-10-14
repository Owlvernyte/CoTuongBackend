namespace CoTuongBackend.Application.Rooms.Dtos;

public sealed record CreateRoomDto
{
    public string? Password { get; set; }
    public Guid HostUserId { get; set; }
}
