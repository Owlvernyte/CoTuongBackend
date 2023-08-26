using CoTuongBackend.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CoTuongBackend.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }
}
