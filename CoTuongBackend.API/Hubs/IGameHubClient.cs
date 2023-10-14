using CoTuongBackend.Application.Users;
using CoTuongBackend.Domain.Entities.Games;

namespace CoTuongBackend.API.Hubs;

public interface IGameHubClient
{
    Task LoadBoard(List<List<Piece?>> squares);
    Task Joined(UserDto userDto);
    Task JoinFailed(string roomCode);
    Task Left(UserDto userDto);
    Task Moved(Coordinate source, Coordinate destination, bool turn);
    Task MoveFailed(Coordinate source, Coordinate destination);
}
