using CoTuongBackend.Domain.Entities;
using CoTuongBackend.Domain.Interfaces;
using CoTuongBackend.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CoTuongBackend.Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly string _tokenKey;
    private readonly ApplicationDbContext _context;
    private readonly TimeSpan _tokenLifespan;
    private readonly SigningCredentials _signingCredentials;

    public TokenService(IConfiguration configuration, ApplicationDbContext context)
    {
        _tokenKey = configuration["TokenKey"]!;
        _context = context;
        _tokenLifespan = TimeSpan.FromHours(5);
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenKey));
        _signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
    }

    public string CreateToken(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
            new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.Add(_tokenLifespan),
            SigningCredentials = _signingCredentials,
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public DateTime GetExpireDate(string token)
    {
        var jwtToken = new JwtSecurityToken(token);
        if (token == null)
            return DateTime.Now;
        return jwtToken.ValidTo.ToUniversalTime();
    }
}
