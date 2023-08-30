using CoTuongBackend.Application.Users;
using CoTuongBackend.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace CoTuongBackend.Application.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    public AccountDTO Login(string userNameOrEmail, string password)
    {
        throw new NotImplementedException();
    }

    public AccountDTO Register(string userName, string email, string password, string confirmPassword)
    {
        throw new NotImplementedException();
    }
}
