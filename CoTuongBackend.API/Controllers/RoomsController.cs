﻿using CoTuongBackend.API.Hubs;
using CoTuongBackend.Application.Rooms;
using CoTuongBackend.Application.Rooms.Dtos;
using CoTuongBackend.Domain.Entities.Games;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Immutable;
using System.Text;

namespace CoTuongBackend.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoomsController : ControllerBase
{
    private readonly IRoomService _roomService;

    public RoomsController(IRoomService roomService)
    {
        _roomService = roomService;
    }

    [HttpPost("join")]
    public async Task<IActionResult> Join(JoinRoomDto request)
    {
        await _roomService.Join(request).ConfigureAwait(false);
        return NoContent();
    }

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
