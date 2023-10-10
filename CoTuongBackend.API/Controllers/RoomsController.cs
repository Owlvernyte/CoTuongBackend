using CoTuongBackend.API.Hubs;
using Microsoft.AspNetCore.Mvc;

namespace CoTuongBackend.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoomsController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        await Task.Delay(0);
        return Ok(GameHub.Boards.Select(x => new
        {
            RoomId = x.Key,
            Squares = x.Value
        }).ToList());
    }
}
