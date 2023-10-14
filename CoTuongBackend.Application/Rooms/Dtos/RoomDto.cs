using CoTuongBackend.Application.Users;

namespace CoTuongBackend.Application.Rooms.Dtos;

public sealed record RoomDto(Guid Id, string Code, int CountUser, string? Password, UserDto HostUser, UserDto? OpponentUser)
{
    public string[]? Board { get; set; }
};
