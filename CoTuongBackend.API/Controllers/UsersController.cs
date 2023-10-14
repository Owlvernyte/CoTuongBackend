using CoTuongBackend.Application.Users;
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
        => Ok(await _userService.Register(registerDTO.Username, registerDTO.Email, registerDTO.Password, registerDTO.ConfirmPassword));

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDTO)
        => Ok(await _userService.Login(loginDTO.UserNameOrEmail, loginDTO.Password));

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChagePasswordDto chagePasswordDTO)
        => Ok(await _userService.ChangePassword(chagePasswordDTO.UserNameOrEmail, chagePasswordDTO.NewPassword, chagePasswordDTO.ConfirmPassword, chagePasswordDTO.OldPassword));

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var firstUser = await _context.Users
            .OrderBy(x => x.CreatedAt)
            .FirstOrDefaultAsync();
        if (firstUser is null)
            return NotFound();
        var token = _tokenService.CreateToken(firstUser);
        return Ok(token);
    }

    [Authorize]
    [HttpGet("check-authorize")]
    public async Task<ActionResult<AccountDto>> CheckAuthorize()
        => Ok(await _userService.CheckAuthorize());

    [HttpGet("users")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        => Ok(await _context.Users
            .Select(u => new UserDto(u.Id, u.UserName, u.Email))
            .ToListAsync());
}
