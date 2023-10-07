using CoTuongBackend.API.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace CoTuongBackend.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class MatchesController : ControllerBase
{
    private readonly IHubContext<GameHub, IGameHubClient> _gameHub;

    public MatchesController(IHubContext<GameHub, IGameHubClient> gameHub)
    {
        _gameHub = gameHub;
    }

    [HttpGet("move")]
    public async Task<IActionResult> Move()
    {
        await _gameHub.Clients.All.Moved("Toi da di nuoc nay bang api");
        return NoContent();
    }
}
