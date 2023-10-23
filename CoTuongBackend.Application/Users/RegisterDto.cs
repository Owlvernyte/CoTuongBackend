namespace CoTuongBackend.Application.Users;

public sealed record RegisterDto
{
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string Email { get; set; }
    public required string ConfirmPassword { get; set; }
}
