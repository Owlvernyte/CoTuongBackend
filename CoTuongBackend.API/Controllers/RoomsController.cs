using CoTuongBackend.API.Hubs;
using CoTuongBackend.Domain.Entities.Games;
using Microsoft.AspNetCore.Mvc;
using System.Text;

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
            Squares = ConvertToBoardArray(x.Value.Squares)
        }).ToList());
    }
    public static string[] ConvertToBoardArray(List<List<Piece?>> initSquares)
    {
        var boardArray = new List<string>();

        foreach (var row in initSquares)
        {
            var sb = new StringBuilder();
            foreach (var piece in row)
            {
                if (piece == null)
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
