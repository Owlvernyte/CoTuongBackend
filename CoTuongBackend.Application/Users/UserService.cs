using CoTuongBackend.Application.Users;
using CoTuongBackend.Domain.Entities;
using CoTuongBackend.Domain.Interfaces;
using FluentValidation;
using FluentValidation.Results;
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
    public async Task<AccountDTO> Login(string userNameOrEmail, string password)
    {
        if (!(await _userManager.Users.AnyAsync(u => u.UserName == userNameOrEmail)) && !(await _userManager.Users.AnyAsync(u => u.Email == userNameOrEmail)))
        {
            // TODO: Check UserName
            throw new UnauthorizedAccessException();
        }
        var user = await _userManager.Users.SingleOrDefaultAsync(u => u.UserName == userNameOrEmail || u.Email == userNameOrEmail);
        if (await _userManager.CheckPasswordAsync(user!, password))
        {
            return new AccountDTO
            {
                Id = user!.Id,
                UserName = user.UserName!,
                Email = user.Email!,
                Token = _tokenService.CreateToken(user)
            };
        }
        throw new UnauthorizedAccessException();
    }

    public async Task<AccountDTO> Register(string userName, string email, string password, string confirmPassword)
    {
        var validationFailures = new List<ValidationFailure>();

        if (userName.Length > 10)
            validationFailures
                    .Add(new ValidationFailure("UserName", "User name must be less than 10 characters"));

        if (await _userManager.Users.AnyAsync(u => u.UserName == userName))
            validationFailures
                .Add(new ValidationFailure("UserName", "User name already exists"));

        if (await _userManager.Users.AnyAsync(u => u.Email == email))
            validationFailures
                .Add(new ValidationFailure("Email", "Email already exists"));

        if (password != confirmPassword)
            validationFailures
                .Add(new ValidationFailure("ConfirmPassword", "Confirm password not match"));

        if (validationFailures.Any())
            throw new ValidationException(validationFailures);

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

        throw new InvalidOperationException();
    }
    public async Task<AccountDTO> ChangePassword(string userNameOrEmail, string newPassword, string confirmPassword, string oldPassword)
    {
        var validationFailures = new List<ValidationFailure>();

        if (newPassword != confirmPassword)
            validationFailures
                .Add(new ValidationFailure("ConfirmPassword", "Confirm password not match"));

        if (validationFailures.Any())
            throw new ValidationException(validationFailures);
        if (!(await _userManager.Users.AnyAsync(u => u.UserName == userNameOrEmail)) && !(await _userManager.Users.AnyAsync(u => u.Email == userNameOrEmail)))
        {
            // TODO: Check UserName
            throw new UnauthorizedAccessException();
        }
        var user = await _userManager.Users.SingleOrDefaultAsync(u => u.UserName == userNameOrEmail || u.Email == userNameOrEmail);
        if (await _userManager.CheckPasswordAsync(user!, oldPassword))
        {
            await _userManager.RemovePasswordAsync(user!);
            await _userManager.AddPasswordAsync(user!, newPassword);
            return new AccountDTO
            {
                Id = user!.Id,
                UserName = user.UserName!,
                Email = user.Email!,
                Token = _tokenService.CreateToken(user)
            };
        }
        else
        {
            throw new UnauthorizedAccessException();
        }
        throw new InvalidOperationException();
    }

}
