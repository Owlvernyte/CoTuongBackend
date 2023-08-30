﻿using CoTuongBackend.Application.Users;
using CoTuongBackend.Domain.Entities;
using CoTuongBackend.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

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
    public Task<AccountDTO> Login(string userNameOrEmail, string password)
    {
        throw new NotImplementedException();
    }

    public async Task<AccountDTO> Register(string userName, string email, string password, string confirmPassword)
    {
        if (await _userManager.Users.AnyAsync(u => u.UserName == userName))
        {
            // TODO: Check UserName
            throw new ValidationException();
        }

        if (await _userManager.Users.AnyAsync(u => u.Email == email))
        {
            // TODO: Check Email
            throw new ValidationException();
        }

        if (password != confirmPassword)
        {
            // TODO: Check confirmPassword
            throw new ValidationException();
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

        throw new ValidationException();
    }
}
