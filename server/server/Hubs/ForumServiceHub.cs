using Microsoft.AspNetCore.SignalR;

namespace RestApiServer.Hubs
{
    public class ForumServiceHub : Hub
    {
        public async Task GetForumStats(string userId)
        {
            await Clients.Caller.SendAsync("GetForumStats", userId);
        }
        public override async Task OnConnectedAsync()
        {
            var context = Context.GetHttpContext();
            if(context == null || context.Request.Query.ContainsKey("userId") == false) 
            {
                return;
            }
            string userId = context.Request.Query["userId"]!;

            //Add this user Id to the group.
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);

            await Clients.Caller.SendAsync("GetForumStats", userId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {   
            var context = Context.GetHttpContext();
            if(context == null || context.Request.Query.ContainsKey("userId") == false) 
            {
                return;
            }
            string userId = context.Request.Query["userId"]!;

            //Remove this user Id from the group.
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);

            await Clients.Caller.SendAsync("GetForumStats", userId);
            await base.OnDisconnectedAsync(exception);
        }
    }
}