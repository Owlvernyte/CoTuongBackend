using CoTuongBackend.API.Hubs;
using CoTuongBackend.Application.Rooms;
using CoTuongBackend.Application.Rooms.Dtos;
using CoTuongBackend.Domain.Entities.Games;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Immutable;
using System.Text;

namespace CoTuongBackend.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class RoomsController : ControllerBase
{
    private readonly IRoomService _roomService;

    public RoomsController(IRoomService roomService)
        => _roomService = roomService;

    [Authorize]
    [HttpPost("join")]
    public async Task<IActionResult> Join(JoinRoomDto request)
    {
        await _roomService.Join(request).ConfigureAwait(false);
        return NoContent();
    }

    [Authorize]
    [HttpPost("leave")]
    public async Task<IActionResult> Leave(LeaveRoomDto request)
    {
        await _roomService.Leave(request).ConfigureAwait(false);
        return NoContent();
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Post(CreateRoomDto request)
    {
        var roomId = await _roomService.Create(request);
        var roomDto = await _roomService.Get(roomId);

        var domain = HttpContext.Request.GetDisplayUrl();
        var routeTemplate = ControllerContext.ActionDescriptor.AttributeRouteInfo!.Template;

        return Created($"{domain}/{routeTemplate}/{roomId}", roomDto);
    }

    [HttpGet]
    public async Task<ActionResult<ImmutableList<RoomDto>>> Get()
    {
        var roomDtos = await _roomService.Get();
        foreach (var roomDto in roomDtos)
        {
            var hasBoard = GameHub.Boards.TryGetValue(roomDto.Code, out var board);
            if (!hasBoard) continue;
            if (board is null) continue;
            roomDto.Board = ConvertToBoardArray(board.Squares);
        }
        return Ok(roomDtos);
    }
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RoomDto>> Get(Guid id)
    {
        var roomDto = await _roomService.Get(id);
        var hasBoard = GameHub.Boards.TryGetValue(roomDto.Code, out var board);
        if (hasBoard && board is { })
        {
            roomDto.Board = ConvertToBoardArray(board.Squares);
        }
        return Ok(roomDto);
    }

    [HttpGet("{code}")]
    public async Task<ActionResult<RoomDto>> Get(string code)
    {
        var roomDto = await _roomService.Get(code);
        var hasBoard = GameHub.Boards.TryGetValue(roomDto.Code, out var board);
        if (hasBoard && board is { })
        {
            roomDto.Board = ConvertToBoardArray(board.Squares);
        }
        return Ok(roomDto);
    }
    [Authorize]
    [HttpDelete("{code}")]
    public async Task<ActionResult<RoomDto>> Delete(string code)
    {
        var roomDto = await _roomService.Get(code);
        var hasBoard = GameHub.Boards.TryGetValue(roomDto.Code, out var board);
        if (hasBoard && board is { })
        {
            roomDto.Board = ConvertToBoardArray(board.Squares);
        }
        await _roomService.Delete(code).ConfigureAwait(false);
        return Ok(roomDto);
    }
    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<RoomDto>> Delete(Guid id)
    {
        var roomDto = await _roomService.Get(id);
        var hasBoard = GameHub.Boards.TryGetValue(roomDto.Code, out var board);
        if (hasBoard && board is { })
        {
            roomDto.Board = ConvertToBoardArray(board.Squares);
        }
        await _roomService.Delete(id).ConfigureAwait(false);
        return Ok(roomDto);
    }

    public static string[] ConvertToBoardArray(List<List<Piece?>> initSquares)
    {
        var boardArray = new List<string>();

        foreach (var row in initSquares)
        {
            var sb = new StringBuilder();
            foreach (var piece in row)
            {
                if (piece is null)
                {
                    sb.Append("__");
                }
                else
                {
                    sb.Append(piece.Signature + "_");
                }
            }
            boardArray.Add(sb.ToString()[..(sb.Length - 1)]);
        }

        return boardArray.ToArray();
    }

}
