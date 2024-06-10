using Microsoft.AspNetCore.SignalR;

namespace Fleet_Management
{
   
        public class RouteHistoryHub : Hub
        {
            public async Task SendMessage(string message)
            {
                await Clients.All.SendAsync("ReceiveMessage", message);
            }
        }
    
}




