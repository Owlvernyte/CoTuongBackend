namespace CoTuongBackend.Application.Users;

public record AccountDto
{
    public Guid Id { get; set; }
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public string? Token { get; set; }
}
