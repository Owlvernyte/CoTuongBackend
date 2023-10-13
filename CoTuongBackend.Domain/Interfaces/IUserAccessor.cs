namespace CoTuongBackend.Domain.Services;

public interface IUserAccessor
{
    string? Email { get; }
    Guid Id { get; }
    string? Jwt { get; }
    IEnumerable<string> Roles { get; }
    string? UserName { get; }
}