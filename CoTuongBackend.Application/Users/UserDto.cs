namespace CoTuongBackend.Application.Users;

public sealed record UserDto(Guid Id, string? UserName, string? Email);
