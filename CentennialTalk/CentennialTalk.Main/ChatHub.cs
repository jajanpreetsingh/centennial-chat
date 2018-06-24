using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace CentennialTalk.Main
{
    public class ChatHub : Hub
    {
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