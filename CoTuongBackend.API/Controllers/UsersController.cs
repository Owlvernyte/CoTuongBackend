﻿using CoTuongBackend.Application.Interfaces;
using CoTuongBackend.Domain.Interfaces;
using CoTuongBackend.Infrastructure.Persistence;
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
}
