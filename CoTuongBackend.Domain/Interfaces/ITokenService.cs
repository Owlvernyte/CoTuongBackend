using CoTuongBackend.Domain.Entities;

namespace CoTuongBackend.Domain.Interfaces;
public interface ITokenService
{
    string CreateToken(ApplicationUser user);
    DateTime GetExpireDate(string token);
}
