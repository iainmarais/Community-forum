using Microsoft.EntityFrameworkCore;
using RestApiServer.CommonEnums;
using RestApiServer.Core.Errorhandler;
using RestApiServer.Database.Db;
using RestApiServer.Db;
using RestApiServer.Db.Users;
using RestApiServer.Dto.Admin;
using RestApiServer.Dto.App;
using RestApiServer.Endpoints.ApiResponses;
using RestApiServer.Endpoints.Dto.Admin;

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

        public static async Task<UserBasicInfo> EditUserAsync(string adminUserId, EditUserRequest req)
        {
            using var db = new AppDbContext();
            //As an added safeguard, check if the current user is an administrator.
            var adminUser = await db.Users.SingleOrDefaultAsync(u => u.AdminUserId == adminUserId);
            if (adminUser == null || adminUser!.RoleId != "Admin")
            {
                throw ClientInducedException.MessageOnly("User is not an administrator");
            }

            //Find the user in the database
            var user = await db.Users.SingleAsync(u => u.UserId == req.UserId);
            user.UserFirstname = req.UserFirstname;
            user.UserLastname = req.UserLastname;
            //Add more props to update as needed.

            //Update the found user and save changes.
            db.Update(user);
            await db.SaveChangesAsync();

            var res = new UserBasicInfo()
            {
                User = user
            };
            return res;
        }

        public static async Task<PaginatedData<List<UserFullInfo>, UserSummary>> GetUsersAsync(string adminUserId, int pageNumber, int rowsPerPage, string? searchTerm)
        {
            using var db = new AppDbContext();

            //As an added safeguard, check if the current user is an administrator.
            var adminUser = await db.Users.SingleOrDefaultAsync(u => u.AdminUserId == adminUserId);
            if (adminUser == null || adminUser!.RoleId != "Admin")
            {
                throw ClientInducedException.MessageOnly("User is not an administrator");
            }

            var usersQuery = from u in db.Users
                             join r in db.Roles on u.RoleId equals r.RoleId
                             //Add any contacts here if needed. For Admin portal this might be essential in tracking down users associated with a particular user.
                             select new UserFullInfo
                             {
                                 User = u,
                                 Role = r
                             };

            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                usersQuery = (from u in usersQuery
                              where u.User.UserFirstname.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase)
                              || u.User.UserLastname.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase)
                              || u.User.Username.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase)
                              select u);
            }


            var filteredTotal = await usersQuery.CountAsync();

            var skip = (pageNumber - 1) * rowsPerPage;
            var userRows = await usersQuery.Skip(skip).Take(rowsPerPage).ToListAsync();

            int totalPages = (filteredTotal + rowsPerPage - 1) / rowsPerPage;

            return new()
            {
                Rows = userRows,
                PageNumber = pageNumber,
                RowsPerPage = rowsPerPage,
                TotalPages = totalPages,
                Summary = new()
                {  
                    TotalUsers = filteredTotal
                }
            };
        }

        public static async Task<PaginatedData<List<BannedUserBasicInfo>, BannedUserSummary>> GetBannedUsersAsync(string adminUserId, int pageNumber, int rowsPerPage, string? searchTerm)
        {
            using var db = new AppDbContext();

            var user = await db.Users.SingleAsync(u => u.UserId == adminUserId);
            if(user.RoleId != "Admin")
            {
                throw ClientInducedException.MessageOnly("User is not an administrator");
            }

            var bannedUsersQuery = from bu in db.BannedUsers
                                    join u in db.Users on bu.UserId equals u.UserId
                                   select new BannedUserBasicInfo
                                   {
                                       BannedUser = bu,
                                       User = u
                                   };
            if(!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                bannedUsersQuery = (from u in bannedUsersQuery
                                    where u.User.UserFirstname.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase)
                                    || u.User.UserLastname.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase)
                                    || u.User.Username.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase)
                                    || u.BannedUser.BanExpirationDate.ToString().Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase)
                                    || u.BannedUser.BanType.ToString().Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase)
                                    select u);
            }

            var filteredTotal = await bannedUsersQuery.CountAsync();
            
            var filteredPermanentlyBannedUsers = await bannedUsersQuery.CountAsync(bu => bu.BannedUser.BanType == BanType.Permanent);

            var skip = (pageNumber - 1) * rowsPerPage;
            var bannedUserRows = await bannedUsersQuery.Skip(skip).Take(rowsPerPage).ToListAsync();

            int totalPages = (filteredTotal + rowsPerPage - 1) / rowsPerPage;

            return new()
            {
                Rows = bannedUserRows,
                PageNumber = pageNumber,
                RowsPerPage = rowsPerPage,
                TotalPages = totalPages,
                Summary = new()
                {
                    TotalBannedUsers = filteredTotal,
                    PermanentlyBannedUsers = filteredPermanentlyBannedUsers
                }
            };
        }
    }
}