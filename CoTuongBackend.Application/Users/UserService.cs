using CoTuongBackend.Application.Users;
using CoTuongBackend.Domain.Entities;
using CoTuongBackend.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CoTuongBackend.Application.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;

    public UserService(UserManager<ApplicationUser> userManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }
    public AccountDTO Login(string userNameOrEmail, string password)
    {
        throw new NotImplementedException();
    }

    public async AccountDTO Register(string userName, string email, string password, string confirmPassword)
    {
        if (await _userManager.Users.AnyAsync(u => u.UserName == userName))
        {
            // TODO: Check UserName
        }

        if (await _userManager.Users.AnyAsync(u => u.Email == email))
        {
            // TODO: Check Email
        }

        if (password != confirmPassword)
        {
            // TODO: Check confirmPassword
        }

        var user = new ApplicationUser
        {
            UserName = userName,
            Email = email
        };

        var result = await _userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            return new AccountDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Token = _tokenService.CreateToken(user)
            };
        }

        return new AccountDTO { Email = "", UserName = "" };
    }
}
