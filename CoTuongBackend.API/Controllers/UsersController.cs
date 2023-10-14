﻿using CoTuongBackend.Application.Users;
using CoTuongBackend.Domain.Interfaces;
using CoTuongBackend.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoTuongBackend.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ApplicationDbContext _context;
    private readonly ITokenService _tokenService;

    public UsersController(IUserService userService, ApplicationDbContext context, ITokenService tokenService)
    {
        _userService = userService;
        _context = context;
        _tokenService = tokenService;
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDTO)
    {
        return Ok(await _userService.Register(registerDTO.Username, registerDTO.Email, registerDTO.Password, registerDTO.ConfirmPassword));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDTO)
    {
        return Ok(await _userService.Login(loginDTO.UserNameOrEmail, loginDTO.Password));
    }
    [HttpPost("change-password")]
    public async Task<IActionResult> CHangePassword([FromBody] ChagePasswordDto chagePasswordDTO)
    {
        return Ok(await _userService.ChangePassword(chagePasswordDTO.UserNameOrEmail,chagePasswordDTO.NewPassword,chagePasswordDTO.ConfirmPassword,chagePasswordDTO.OldPassword));
    }
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var firstUser = await _context.Users.FirstOrDefaultAsync();
        if (firstUser == null)
        {
            return NotFound();
        }
        var token = _tokenService.CreateToken(firstUser);
        return Ok(token);
    }
    [Authorize]
    [HttpGet("check-authorize")]
    public int GetNum()
    {
        return 3;
    }
    [HttpGet("users")]
    public async Task<IActionResult> GetUsers()
    {
        return Ok(_context.Users);
    }
}
