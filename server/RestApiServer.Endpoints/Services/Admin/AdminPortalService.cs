using Microsoft.EntityFrameworkCore;
using RestApiServer.Common.Services;
using RestApiServer.CommonEnums;
using RestApiServer.Core.Errorhandler;
using RestApiServer.Database.Db;
using RestApiServer.Db;
using RestApiServer.Db.Users;
using RestApiServer.Dto.Admin;
using RestApiServer.Dto.App;
using RestApiServer.Endpoints.ApiResponses;
using RestApiServer.Endpoints.Dto.Admin;
using RestApiServer.Endpoints.Helpers;
using Serilog;

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
                //If database uses MariaDb: Create an if-check here, somehow we need to determine which database we are using... If this works, use it in all paginated data methods.
                usersQuery = (from u in usersQuery
                            where u.User.UserFirstname.ToLower().Contains(searchTerm)
                            || u.User.UserLastname.ToLower().Contains(searchTerm)
                            || u.User.Username.ToLower().Contains(searchTerm)
                            select u);
                //Else if MySQL: Still to be implemented.
            }
            var filteredTotal = await usersQuery.CountAsync();

            var skip = (pageNumber - 1) * rowsPerPage;
            var userRows = await usersQuery.Skip(skip).Take(rowsPerPage).ToListAsync();

            var totalPages = (filteredTotal + rowsPerPage - 1) / rowsPerPage;

            return new PaginatedData<List<UserFullInfo>, UserSummary>()
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

        public static async Task<BannedUserBasicInfo> BanUserAsync(string adminUserId, string userId, BanUserRequest req)
        {
            using var db = new AppDbContext();

            //As an added safeguard, check if the current user is an administrator.
            var adminUser = await db.Users.SingleOrDefaultAsync(u => u.AdminUserId == adminUserId);
            if (adminUser == null || adminUser!.RoleId != "Admin")
            {
                throw ClientInducedException.MessageOnly("User is not an administrator");
            }

            //Find the requested user in the database
            var user = await db.Users.SingleOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                throw ClientInducedException.MessageOnly("User not found");
            }

            //Check for any existing bans. This should return a null or zero count
            var existingBan = await db.BannedUsers.Where(b => b.UserId == userId).CountAsync();
            if (existingBan > 0)
            {
                throw ClientInducedException.MessageOnly("User is already banned");
            }
            var newBanType = Enum.TryParse(req.BanType, out BanType banType) ? banType : BanType.Permanent;
            var newBan = new BannedUserEntry()
            {
                UserId = userId,
                BanType = newBanType,
                BanExpirationDate = req.BanExpirationDate,
                BanReason = req.BanReason
            };
            db.BannedUsers.Add(newBan);
            await db.SaveChangesAsync();
            return new BannedUserBasicInfo()
            {
                BannedUser = newBan,
                User = user
            };
        }

        public static async Task DeleteUserAsync(string adminUserId, string userId)
        {
            using var db = new AppDbContext();
            
            var userToDelete = await db.Users.SingleOrDefaultAsync(u => u.UserId == userId);
            //Make sure the user to delete is not the same as the admin user (the one triggering this code.)
            var adminUser = await db.Users.SingleOrDefaultAsync(a => a.UserId == adminUserId);
            if(userToDelete == adminUser)
            {
                throw ClientInducedException.MessageOnly("Can't delete the admin user.");
            }
            if(userToDelete == null)
            {
                throw ClientInducedException.MessageOnly("User not found");
            }
            //Finally, remove the user.
            db.Users.Remove(userToDelete);
        }

        public static async Task<List<RoleEntry>> GetRolesAsync()
        {
            using var db = new AppDbContext();

            var res = await db.Roles.ToListAsync();

            return res;
        }

        public static async Task AssignRoleAsync(string userId, AssignRoleRequest request)
        {
            using var db = new AppDbContext();

            var user = await db.Users.SingleAsync(u => u.UserId == userId);
            if(user == null)
            {
                throw ClientInducedException.MessageOnly("User not found");
            }
            //Look for the role using either its id or name.
            var roleToAssign = await db.Roles.SingleAsync(r => r.RoleId == request.SelectedRoleId || r.RoleName == request.SelectedRoleName);

            if(roleToAssign == null)
            {
                throw ClientInducedException.MessageOnly("Role not found");
            }
            user.RoleId = roleToAssign.RoleId;
            await db.SaveChangesAsync();
        }

        public static async Task<UserBasicInfo> AddUserAsync(AddUserRequest req)
        {
            using var db = new AppDbContext();

            var salt = AuthService.GenerateSalt();
            var user = new UserEntry()
            {
                UserId = Guid.NewGuid().ToString(),
                ForumUserId = Guid.NewGuid().ToString(),
                Username = req.Username,
                HashedPassword = AuthService.HashPassword(req.Password, salt),
                EmailAddress = req.EmailAddress,
                RoleId = req.RoleId ?? "User",
                RegistrationTime = DateTime.UtcNow,
            };
            db.Users.Add(user);
            await db.SaveChangesAsync();
            return new UserBasicInfo()
            {
                User = user
            };
        }
    }
}