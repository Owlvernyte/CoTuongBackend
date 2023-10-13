using CoTuongBackend.Domain.Services;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CoTuongBackend.Infrastructure.Services;

public sealed class UserAccessor : IUserAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserAccessor(IHttpContextAccessor httpContextAccessor)
        => _httpContextAccessor = httpContextAccessor;
    public Guid Id => Guid.Parse(_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? Guid.Empty.ToString());
    public string? UserName => _httpContextAccessor.HttpContext?.User.Identity?.Name;
    public string? Email => _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);
    public string? Jwt =>
        _httpContextAccessor.HttpContext!.Request.Headers["Authorization"].ToString() is { }
        && _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().StartsWith("Bearer ")
        ? _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString()["Bearer ".Length..].Trim() : null;
    public IEnumerable<string> Roles => _httpContextAccessor.HttpContext?.User.Claims
        .Where(x => x.Type == ClaimTypes.Role)
        .Select(x => x.Value)
        .ToList() ?? Enumerable.Empty<string>();
}
