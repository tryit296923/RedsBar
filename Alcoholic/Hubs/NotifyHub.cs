using Microsoft.AspNetCore.SignalR;
using NuGet.Protocol.Plugins;

namespace Alcoholic.Hubs
{
    public class NotifyHub : Hub
    {
        public Task SendMessage(string user,string message)
        {
            return Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        public override Task OnConnectedAsync()
        {
            
            return base.OnConnectedAsync();
        }
    }
}
