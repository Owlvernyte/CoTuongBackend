namespace CoTuongBackend.Application.Users;

public record LoginDto
{
    public required string UserNameOrEmail { get; set; }
    public required string Password { get; set; }
}
