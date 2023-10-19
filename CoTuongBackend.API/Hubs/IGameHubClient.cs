using CoTuongBackend.Application.Games.Enums;
using CoTuongBackend.Application.Users;
using CoTuongBackend.Domain.Entities.Games;

namespace CoTuongBackend.API.Hubs;

public interface IGameHubClient
{
    Task LoadBoard(List<List<Piece?>> squares, bool isHostRed, bool isRedTurn);
    Task Joined(UserDto userDto);
    Task JoinFailed(string roomCode);
    Task Left(UserDto userDto);
    Task Moved(Coordinate source, Coordinate destination, bool isRedTurn);
    Task MoveFailed(MoveStatus status);
    Task Chatted(string message, string roomCode, UserDto userDto);
    Task Ended(bool isWinnerRed, UserDto winner);
}
