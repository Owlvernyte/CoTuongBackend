namespace CoTuongBackend.API.Hubs;

public interface IGameHubClient
{
    Task Joined(string message);
    Task Left(string message);
    Task Moved(string result);
}
