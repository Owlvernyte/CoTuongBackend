using CoTuongBackend.Domain.Services;
using CoTuongBackend.Infrastructure.Constants;
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
    public string? Jwt
    {
        get
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext is null) return string.Empty;
            var hasCookieToken = httpContext.Request.Cookies.TryGetValue(AuthenticationConstants.CookieUserToken, out var cookieToken);
            if (hasCookieToken && cookieToken is { })
            {
                return cookieToken;
            }
            var hasHeaderToken = httpContext.Request.Headers.TryGetValue("Authorization", out var authoriaztionHeader);
            if (hasHeaderToken && authoriaztionHeader is { })
            {
                return authoriaztionHeader
                  .ToString()
                  .Replace("Bearer ", string.Empty);
            }
            var hasQueryToken = httpContext.Request.Query.TryGetValue(AuthenticationConstants.QueryUserToken, out var accessToken);
            if (hasQueryToken && accessToken is { })
            {
                return accessToken.ToString();
            }
            return string.Empty;
        }
    }

    public IEnumerable<string> Roles => _httpContextAccessor.HttpContext?.User.Claims
        .Where(x => x.Type == ClaimTypes.Role)
        .Select(x => x.Value)
        .ToList() ?? Enumerable.Empty<string>();
}
