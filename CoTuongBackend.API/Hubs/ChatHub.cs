using Microsoft.AspNetCore.SignalR;

namespace CoTuongBackend.API.Hubs
{
    public class ChatHub : Hub<IChatHubClient>
    {
        public override Task OnConnectedAsync()
        {
            Console.WriteLine("Nguoi choi " + Context.ConnectionId + " da ket noi vao hub");

            Clients.All.Joined("Nguoi choi " + Context.ConnectionId + " da tham gia phong!");

            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine("Nguoi choi " + Context.ConnectionId + " da ngat ket noi");

            Clients.All.Left("Nguoi choi " + Context.ConnectionId + " da roi phong!");

            return base.OnDisconnectedAsync(exception);
        }
        public Task Chat(string message)
        {

            Console.WriteLine("Nguoi choi " + Context.ConnectionId + " da chat" + message);

            Clients.All.Chat("Nguoi choi " + Context.ConnectionId + " da chat" + message);

            return Task.CompletedTask;
        }
    }
}
