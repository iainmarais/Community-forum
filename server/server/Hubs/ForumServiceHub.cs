using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using RestApiServer.Dto.App;
using RestApiServer.Services;

namespace RestApiServer.Hubs
{
    public class ForumServiceHub : Hub
    {
        public async Task GetForumStats(string userId)
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

        public async Task ProcessIncomingMessage(string jsonString)
        {
            try
            {
                var messages = JsonSerializer.Deserialize<List<SignalRMessage>>(jsonString);
                if(messages != null && messages.Any())
                {
                    foreach(var message in messages)
                    {
                        if(message.Target == "GetForumStats")
                        {
                            await GetForumStats(message.Arguments[0]);
                        }
                    }
                }
            }
            catch(JsonException ex)
            {
                Console.WriteLine($"Deserialisation of the incoming arg string failed.\n{ex.Message}");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Could not process incoming string.\n{ex.Message}");
            }
        }
    }

    public class SignalRMessage
    {
        public string Protocol { get; set; } = "";
        public int Version { get; set; }
        public int Type  { get; set; }
        public string Target { get; set; } = "";
        public string UserId { get; set; } = "";
        public string[] Arguments { get; set; } = [];
    }
}