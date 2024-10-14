using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using RestApiServer.Db;
using RestApiServer.Db.Users;
using RestApiServer.Dto.App;
using RestApiServer.Dto.Forum;
using RestApiServer.Services;
using RestApiServer.Services.Forum.Categories;
using Serilog;
using System.Threading.Tasks;

namespace RestApiServer.Hubs
{
    public class ForumStatsHub : Hub
    {
        public async Task<ForumAppState> GetForumAppStateAsync(string userId)
        {
            Log.Information($"User id sent: {userId}, Getting forum app state");
            using var _dbContext = new AppDbContext();

            var forumStats = new ForumStats();

            forumStats.TotalPosts = await _dbContext.Posts.CountAsync();
            forumStats.TotalUsers = await _dbContext.Users.CountAsync();
            forumStats.TotalThreads = await _dbContext.Threads.CountAsync();
            forumStats.MostActiveUsers = await _dbContext.Users
                .Where(u => u.TotalPosts >= 500)
                .Select(u => new UserBasicInfo()
                {
                    User = u,
                })
                .ToListAsync();
            forumStats.TotalTopics = await _dbContext.Topics.CountAsync();
            forumStats.PopularTopics = (await TopicService.GetPopularForumTopicsAsync()).Count;

            var user = await UserService.GetUserBasicInfoAsync(userId);
            var loggedInUserRoleName = await _dbContext.Roles
                .Where(r => r.RoleId == user.User.RoleId)
                .Select(r => r.RoleName)
                .FirstOrDefaultAsync();

            var forumAppState = new ForumAppState
            {
                ForumStats = forumStats,
                LoggedInUser = new LoggedInUserInfo
                {
                    UserId = user.User.UserId ?? "",
                    UserFirstname = user.User.UserFirstname ?? "",
                    UserLastname = user.User.UserLastname ?? "",
                    RoleName = loggedInUserRoleName ?? "",
                    UserProfileImageBase64 = user.User.UserProfileImageBase64 ?? ""
                }
            };

            //Send it via SignalR.
            await Clients.Caller.SendAsync("ReceiveForumAppState", forumAppState);

            //In case I need to grab this outside of SignalR.
            return forumAppState;
        }
        public async Task SendStats(string message)
        {
            //Todo: Get the forum stats and send this through via SignalR to the client.
            //Forum stats include the total posts, threads, topics, categories, most active threads, users, etc
            
            await Clients.All.SendAsync("ReceiveStats", message);
        }
    }
}