using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Gateway.Web.Api.SignalR
{
    public class Notification : Hub
    {
        public Task Send(string message)
        {
            return Clients.All.SendAsync("Send", message);
        }
    }
}