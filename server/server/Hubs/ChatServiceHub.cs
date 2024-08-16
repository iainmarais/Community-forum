using Microsoft.AspNetCore.SignalR;
using Serilog;

namespace RestApiServer.Hubs
{
    public class ChatServiceHub : Hub
    {
        private readonly SignalRMessageProcessor _MessageProcessor;
        public ChatServiceHub()
        {
            _MessageProcessor = new SignalRMessageProcessor();
            _MessageProcessor.RegisterHandler("SendMessage", SendMessage);
        }
        public async Task SendMessage(string userId,  List<string>? args)
        {
            if(args != null && args.Count > 0)
            {
                var chatId = args.FirstOrDefault(a => a.StartsWith("chatId:")).Split(":")[1];
                var message = args.FirstOrDefault(a => a.StartsWith("message:")).Split(":")[1];
                await Clients.Group(chatId).SendAsync("ReceiveMessage", userId, chatId, message);
            }
            else
            {
                Log.Error("Message and chat Id not present. Unable to send message.");
            }
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