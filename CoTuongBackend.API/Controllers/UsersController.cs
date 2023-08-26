using CoTuongBackend.Application.Interfaces;
using CoTuongBackend.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace CoTuongBackend.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ApplicationDbContext _context;

    public UsersController(IUserService userService, ApplicationDbContext context)
    {
        _userService = userService;
        _context = context;
    }

    [HttpGet]
    public int Get()
    {
        return _context.Users.Count();
    }
}
