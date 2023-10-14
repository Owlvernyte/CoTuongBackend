using CoTuongBackend.Application.Users;

namespace CoTuongBackend.API.Hubs
{
    public interface IChatHubClient
    {
        Task Joined(UserDto userDto);
        Task Left(UserDto userDto);
        Task Chat(string message, string roomCode, UserDto userDto);
    }
}
