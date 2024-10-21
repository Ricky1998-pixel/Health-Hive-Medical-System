
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Health_Hive_Project_2024.Hubs
{
    public class ChatHub : Hub
    {
        private static readonly ConcurrentDictionary<string, string> UserConnections = new();

        public override Task OnConnectedAsync()
        {
            UserConnections[Context.User.Identity.Name] = Context.ConnectionId;
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(System.Exception exception)
        {
            UserConnections.TryRemove(Context.User.Identity.Name, out _);
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string recipientUserName, string message)
        {
            if (UserConnections.TryGetValue(recipientUserName, out var connectionId))
            {
                await Clients.Client(connectionId).SendAsync("ReceiveMessage", Context.User.Identity.Name, message);
            }
        }
    }

}
