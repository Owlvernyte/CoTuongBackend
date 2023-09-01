namespace CoTuongBackend.Application.Users;

public record LoginDTO
{
    public required string UserNameOrEmail { get; set; }
    public required string Password { get; set; }
}
