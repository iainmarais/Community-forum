using Microsoft.AspNetCore.SignalR;

namespace RestApiServer.Hubs
{
    public class ChatServiceHub : Hub
    {
        public async Task SendMessage(string chatId, string userId, string message)
        {
            await Clients.Group(chatId).SendAsync("ReceiveMessage", chatId, userId, message);
        }

        public override async Task OnConnectedAsync()
        {
            string chatId = Context.GetHttpContext().Request.Query["chatId"];
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            string chatId = Context.GetHttpContext().Request.Query["chatId"];
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId);
         
            await base.OnDisconnectedAsync(exception);
        }
    }
}