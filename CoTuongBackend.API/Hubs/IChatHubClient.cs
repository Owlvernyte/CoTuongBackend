namespace CoTuongBackend.API.Hubs
{
    public interface IChatHubClient
    {
        Task Joined(string message);
        Task Left(string message);
        Task Chat(string message);
    }
}
