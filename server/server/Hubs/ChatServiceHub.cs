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
            var context = Context.GetHttpContext();
            if(context == null || context.Request.Query.ContainsKey("chatId") == false)
            {
                return;
            }
            string chatId = context.Request.Query["chatId"]!;
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var context = Context.GetHttpContext();
            if(context == null || context.Request.Query.ContainsKey("chatId") == false)
            {
                return;
            }
            string chatId = context.Request.Query["chatId"]!;
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId);
         
            await base.OnDisconnectedAsync(exception);
        }
    }
}