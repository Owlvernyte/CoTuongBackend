using CoTuongBackend.Application.Matches;
using CoTuongBackend.Application.Matches.Dtos;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Immutable;

namespace CoTuongBackend.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public sealed class MatchesController : ControllerBase
{
    private readonly IMatchService _matchService;

    public MatchesController(IMatchService matchService)
        => _matchService = matchService;
    [HttpPost]
    public async Task<ActionResult<MatchDto>> Post(CreateMatchWithRoomIdDto request)
    {
        var matchId = await _matchService.Create(request);
        var matchDto = await _matchService.Get(matchId);

        var domain = HttpContext.Request.GetDisplayUrl();
        var routeTemplate = ControllerContext.ActionDescriptor.AttributeRouteInfo!.Template;

        return Created($"{domain}/{routeTemplate}/{matchId}", matchDto);
    }
    [HttpGet]
    public async Task<ActionResult<ImmutableList<MatchDto>>> Get()
        => Ok(await _matchService.Get());
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<MatchDto>> Get(Guid id)
        => Ok(await _matchService.Get(id));
}
