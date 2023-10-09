using CoTuongBackend.Domain.Entities.Games;

namespace CoTuongBackend.API.Hubs;

public interface IGameHubClient
{
    Task Joined(List<List<Piece?>> pieces);
    Task Left(string message);
    Task Moved(Coordinate source, Coordinate destination, bool turn);
    Task MoveFailed(Coordinate source, Coordinate destination);
}
