using Microsoft.EntityFrameworkCore;
using RestApiServer.Db;
using RestApiServer.Db.Users;
using RestApiServer.Dto.Admin;
using RestApiServer.Dto.App;
using RestApiServer.Dto.Forum;
using RestApiServer.Endpoints.ApiResponses;


namespace RestApiServer.Endpoints.Services.Admin
{
    public class AdminPortalService
    {
        public static async Task<AdminPortalAppState> GetAdminPortalAppStateAsync(string userId)
        {
            using var db = new AppDbContext();
            //Current User
            var currentUser = await db.Users.SingleOrDefaultAsync(u => u.UserId == userId);

            //Stats, such as total posts, threads, topics, registered users, gallery items, etc.
            var totalPosts = await db.Posts.CountAsync();
            var totalCategories = await db.Categories.CountAsync();
            var totalBoards = await db.Boards.CountAsync();
            var totalThreads = await db.Threads.CountAsync();
            var totalTopics = await db.Topics.CountAsync();
            var totalUsers = await db.Users.CountAsync();
            var totalGalleryItems = await db.GalleryItems.CountAsync();

            var adminPortalStats = new AdminPortalStats()
            {
                TotalPosts = totalPosts,
                TotalCategories = totalCategories,
                TotalBoards = totalBoards,
                TotalThreads = totalThreads,
                TotalTopics = totalTopics,
                TotalUsers = totalUsers,
                TotalGalleryItems = totalGalleryItems
            };

            var res = new AdminPortalAppState
            {
                CurrentLoggedInUser = new LoggedInUserInfo()
                {
                    UserId = currentUser!.UserId,
                    UserFirstname = currentUser.UserFirstname,
                    UserLastname = currentUser.UserLastname,
                    UserProfileImageBase64 = currentUser.UserProfileImageBase64,
                    RoleName = db.Roles.Single(r => r.RoleId == currentUser.RoleId).RoleName
                },
                AdminPortalStats = adminPortalStats
            };

            return res;
        }      
    }
}