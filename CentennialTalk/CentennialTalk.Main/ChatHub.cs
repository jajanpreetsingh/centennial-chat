using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace CentennialTalk.Main
{
    public class ChatHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            base.OnConnectedAsync();

            return Clients.All.SendAsync("connectionStarted", Context.ConnectionId);
        }

        public Task Send(string jsonMessage)
        {
            return Clients.All.SendAsync("receive", jsonMessage);
        }

        public Task Receive(string jsonMessage)
        {
            return Clients.All.SendAsync("Recieve", jsonMessage);
        }
    }
}