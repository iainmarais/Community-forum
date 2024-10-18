using Microsoft.EntityFrameworkCore;
using RestApiServer.Db;
using RestApiServer.Dto.App;
using RestApiServer.Db.Users;
using RestApiServer.Services.Forum.Categories;
using RestApiServer.Dto.Forum;

namespace RestApiServer.Services.Forum
{
    public class ForumService
    {
        private readonly AppDbContext _dbContext;

        public ForumService()
        {
            _dbContext = new AppDbContext();
        }

        public async Task<ForumAppState> GetForumAppStateAsync(string userId)
        {
            // Initialize forumStats
            var forumStats = new ForumStats();

            // Retrieve data using a single DbContext instance
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

            var res = new ForumAppState
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

            return res;
        }

        public async Task<ForumAppState> GetForumPublicAppStateAsync()
        {
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

            var res = new ForumAppState
            {
                ForumStats = forumStats,
            };

            return res;
        }
    }

}