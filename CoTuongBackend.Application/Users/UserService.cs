﻿using CoTuongBackend.Application.Users;
using CoTuongBackend.Domain.Entities;
using CoTuongBackend.Domain.Interfaces;
using CoTuongBackend.Domain.Services;
using CoTuongBackend.Infrastructure.Constants;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CoTuongBackend.Application.Services;

public sealed class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly IUserAccessor _userAccessor;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(UserManager<ApplicationUser> userManager, ITokenService tokenService, IUserAccessor userAccessor, IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _userAccessor = userAccessor;
        _httpContextAccessor = httpContextAccessor;
    }
    public Task<AccountDto> CheckAuthorization()
    {
        return Task.FromResult(new AccountDto
        {
            Id = _userAccessor.Id,
            UserName = _userAccessor.UserName,
            Email = _userAccessor.Email!,
            Token = _userAccessor.Jwt
        });
    }
    public async Task<AccountDto> Login(string userNameOrEmail, string password)
    {
        if (!(await _userManager.Users.AnyAsync(u => u.UserName == userNameOrEmail)) && !(await _userManager.Users.AnyAsync(u => u.Email == userNameOrEmail)))
        {
            // TODO: Check UserName
            throw new UnauthorizedAccessException();
        }
        var user = await _userManager.Users.SingleOrDefaultAsync(u => u.UserName == userNameOrEmail || u.Email == userNameOrEmail)
            ?? throw new UnauthorizedAccessException();
        if (await _userManager.CheckPasswordAsync(user, password))
        {

            var token = _tokenService.CreateToken(user);
            _httpContextAccessor.HttpContext?.Response.Cookies
                .Append(AuthenticationConstants.CookieUserToken, token);
            return new AccountDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Token = token
            };
        }
        throw new UnauthorizedAccessException();
    }

    public async Task<AccountDto> Register(string userName, string email, string password, string confirmPassword)
    {
        var validationFailures = new List<ValidationFailure>();

        if (userName.Length > 10)
            validationFailures
                    .Add(new ValidationFailure("UserName", "The username must be 10 characters or fewer"));

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
            var token = _tokenService.CreateToken(user);
            _httpContextAccessor.HttpContext?.Response.Cookies
                .Append(AuthenticationConstants.CookieUserToken, token);
            return new AccountDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Token = token
            };
        }

        throw new InvalidOperationException();
    }
    public async Task<AccountDto> ChangePassword(string userNameOrEmail, string newPassword, string confirmPassword, string oldPassword)
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
        var user = await _userManager.Users.SingleOrDefaultAsync(u => u.UserName == userNameOrEmail || u.Email == userNameOrEmail)
            ?? throw new UnauthorizedAccessException();
        if (await _userManager.CheckPasswordAsync(user, oldPassword))
        {
            await _userManager.RemovePasswordAsync(user);
            await _userManager.AddPasswordAsync(user, newPassword);
            var token = _tokenService.CreateToken(user);
            _httpContextAccessor.HttpContext?.Response.Cookies
                .Append(AuthenticationConstants.CookieUserToken, token);
            return new AccountDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Token = token
            };
        }
        else
        {
            throw new UnauthorizedAccessException();
        }
        throw new InvalidOperationException();
    }

}
