using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using RestApiServer.Dto.App;
using RestApiServer.Services;

namespace RestApiServer.Hubs
{
    public class ForumServiceHub : Hub
    {
        private readonly SignalRMessageProcessor _MessageProcessor;

        public ForumServiceHub()
        {
            _MessageProcessor = new SignalRMessageProcessor();

            _MessageProcessor.RegisterHandler("GetForumStats", GetForumStats);
        }

        public async Task GetForumStats(string userId,  List<string>? args)
        {   
            var forumStats = await GetForumStatsForUser(userId);
            await Clients.Caller.SendAsync("GetForumStats", userId);

            await Clients.Caller.SendAsync("UpdateForumStats", userId);
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

        private async Task<ForumStats> GetForumStatsForUser(string userId)
        {
            var res = await ForumService.GetForumAppStateAsync(userId);
            return res.ForumStats;
        }

        //Can this not be turned into a standalone module?
        public async Task ProcessIncomingMessage(string jsonString)
        {
            await _MessageProcessor.ProcessIncomingMessage(jsonString);
        }
    }
}