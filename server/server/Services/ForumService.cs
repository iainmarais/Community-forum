using Microsoft.EntityFrameworkCore;
using RestApiServer.Db;
using RestApiServer.Dto.App;
using RestApiServer.Db.Users;
using RestApiServer.Utils;
using RestApiServer.Services.Categories;

namespace RestApiServer.Services
{
    public class ForumService
    {
        private readonly AppDbContext _dbContext;
        public ForumService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public static async Task<ForumAppState> GetForumAppStateAsync(string userId)
        {
            //As much as I would like to condense this, it seems impossible to achieve with EFCore whining about using the same db context and executing concurrent actions against it.
            var forumStats = new ForumStats();
            string? loggedInUserRoleName = "";
            using(var db = new AppDbContext())
            {
                forumStats.TotalPosts = await db.Messages.CountAsync();
            };
            using(var db2 = new AppDbContext())
            {
                forumStats.TotalUsers = await db2.Users.CountAsync();
            };
            using(var db3 = new AppDbContext())
            {
                forumStats.TotalThreads = await db3.Threads.CountAsync();
            };
            using(var db4 = new AppDbContext())
            {
                forumStats.MostActiveUsers = await db4.Users
                                                        .Where(u => u.TotalPosts >= 500)
                                                        .Select(u => new UserBasicInfo(u))
                                                        .ToListAsync();
            }
            using(var db5 = new AppDbContext())
            {
                forumStats.TotalTopics = await db5.Topics.CountAsync();
            }
            var user = await UserService.GetUserBasicInfoAsync(userId);
            using(var db6 = new AppDbContext())
            {
                loggedInUserRoleName = await db6.Roles.Where(r => r.RoleId == user.RoleId).Select(r => r.RoleName).FirstOrDefaultAsync();
            } 

            {
                forumStats.PopularTopics = TopicService.GetPopularForumTopicsAsync().Result.Count;
            }
            var res = new ForumAppState
            {
                ForumStats = forumStats,
                LoggedInUser = new LoggedInUserInfo
                {
                    UserId = user.UserId ?? "",
                    UserFirstname = user.UserFullname ?? "",
                    UserLastname = user.UserLastname ?? "",
                    RoleName = loggedInUserRoleName ?? "",
                }
            };
            return res;
        }
        
        public static async Task<ForumAppState> GetForumPublicAppStateAsync()
        {
            //As much as I would like to condense this, it seems impossible to achieve with EFCore whining about using the same db context and executing concurrent actions against it.
            var forumStats = new ForumStats();
            using(var db = new AppDbContext())
            {
                forumStats.TotalPosts = await db.Messages.CountAsync();
            };
            using(var db2 = new AppDbContext())
            {
                forumStats.TotalUsers = await db2.Users.CountAsync();
            };
            using(var db3 = new AppDbContext())
            {
                forumStats.TotalThreads = await db3.Threads.CountAsync();
            };
            using(var db4 = new AppDbContext())
            {
                forumStats.MostActiveUsers = await db4.Users
                                                        .Where(u => u.TotalPosts >= 500)
                                                        .Select(u => new UserBasicInfo(u))
                                                        .ToListAsync();
            }
            using(var db5 = new AppDbContext())
            {
                forumStats.TotalTopics = await db5.Topics.CountAsync();
            }

            {
                forumStats.PopularTopics = TopicService.GetPopularForumTopicsAsync().Result.Count;
            }
            var res = new ForumAppState
            {
                ForumStats = forumStats,
            };
            return res;
        }        
    }
}