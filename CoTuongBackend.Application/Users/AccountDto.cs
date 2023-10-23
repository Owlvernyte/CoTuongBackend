namespace CoTuongBackend.Application.Users;

public sealed record AccountDto
{
    public Guid Id { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? Token { get; set; }
}
