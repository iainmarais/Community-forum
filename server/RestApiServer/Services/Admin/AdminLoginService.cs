using Microsoft.EntityFrameworkCore;
using RestApiServer.Core.Errorhandler;
using RestApiServer.Db;
using RestApiServer.Db.Users;
using RestApiServer.Dto.Admin;
using RestApiServer.Dto.AdminLogin;
using RestApiServer.Dto.App;
using RestApiServer.Enums;
using RestApiServer.Utils;

namespace RestApiServer.Services.Admin
{
    public class AdminLoginService
    {
        public static async Task<AdminUserLoginResponse> AdminLoginAsync(AdminUserLoginRequest req)
        {
            using var db = new AppDbContext();
            UserEntry? foundUser = null;
            //User is logging in with an email address.
            if(!string.IsNullOrEmpty(req.UserIdentifier))
            {
                foundUser = await FindUserAsync(db, req.UserIdentifier);
                //User not found or does not exit at this point.
                if (foundUser== null)
                {
                    throw ClientInducedException.MessageOnly("User not found or does not exist.");
                }

                var foundUserRole = await db.Roles.SingleOrDefaultAsync(r => r.RoleId == foundUser!.RoleId);
                if(foundUserRole == null || foundUserRole!.RoleType != RoleType.Admin)
                {
                    throw ClientInducedException.MessageOnly("Access denied. User is not an administrator.");
                }                
                //Handle the user context now.
                ValidateUserContext(req.UserContext);
                return await LoginSuccessResponse(db, foundUser.UserId, foundUser.RoleId, req.UserContext);
            }
            //Invalid username or email address.
            else
            {
                throw ClientInducedException.MessageOnly("Please check your login credentials.")   ;
            }
        }

        private static void ValidateUserContext(string userContext)
        {
            switch(userContext)
            {
                case "admin":
                    return;
                case "chat":
                case "forum":
                    throw ClientInducedException.MessageOnly("Provided user context not valid in this use case.");
                default:
                    throw ClientInducedException.MessageOnly("Invalid user context.");
            }
        }

        private static async Task<UserEntry?> FindUserAsync(AppDbContext db, string userIdentifier)
        {
            if (userIdentifier.Contains("@"))
            {
                // User is logging in with an email address.
                return await (from u in db.Users
                    join r in db.Roles on u.RoleId equals r.RoleId
                    where u.EmailAddress == userIdentifier
                    select u).SingleOrDefaultAsync();
            }
            return await (from u in db.Users
                join r in db.Roles on u.RoleId equals r.RoleId
                where u.Username == userIdentifier
                select u).SingleOrDefaultAsync();
        }

        private static async Task<AdminUserLoginResponse> LoginSuccessResponse(AppDbContext db, string userId, string roleId, string userContext)
        {
            var userResult = await (from u in db.Users 
                                    where u.UserId == userId 
                                    join role in db.Roles on u.RoleId equals role.RoleId
                                    select new 
                                    {
                                        User = u,
                                        Role = role
                                    }).SingleAsync();
            if(userResult == null)
            {
                throw ClientInducedException.MessageOnly("User not found");
            }

            var permissions = await (from up in db.UserPermissions
                                    join sp in db.SystemPermissions on up.SystemPermissionId equals sp.SystemPermissionId
                                    where up.UserId == userId
                                    select sp.SystemPermissionType)
                                    .Distinct().ToListAsync();

            //Permissions for roles
            var rolePermissions = await (from rp in db.RolePermissions
                                    join p in db.Permissions
                                    on rp.PermissionId equals p.PermissionId
                                    where rp.RoleId == roleId
                                    select p.PermissionType)
                                    .Distinct().ToListAsync();                                    
            var user = new
            {
                userResult.User,
                Permissions = permissions,
                RolePermissions = rolePermissions,
                userResult.Role
            }; 
            var existingRefreshTokensForSource = await db.UserRefreshTokens.Where(t => t.AssignedToUserId == userId && t.Source == userContext).ToListAsync();

            if(existingRefreshTokensForSource.Count > 0)
            {
                db.RemoveRange(existingRefreshTokensForSource);
            }
            //Create the user refresh token.
            var(userRefreshToken, userRefreshTokenExpiration) = AuthUtils.GenerateRefreshToken();

            var newRefreshToken = new UserRefreshTokenEntry
            {
                UserRefreshTokenId = DbUtils.GenerateUuid(),
                AssignedToUserId = userId,
                RefreshToken = userRefreshToken,
                RefreshTokenExpirationDate = userRefreshTokenExpiration,
                Source = userContext,
                IsRevoked = false
            };
            await db.UserRefreshTokens.AddAsync(newRefreshToken);

            var (userSessionToken, userSessionTokenExpiration) = userContext switch
            {
                "admin" => AuthUtils.GenerateAdminUserAccessToken(userId, userResult.User.AdminUserId, permissions, new(){ userResult.Role.RoleType }),
                "forum" => AuthUtils.GenerateForumUserAccessToken(userId, userResult.User.ForumUserId, permissions, new(){ userResult.Role.RoleType }),
                //Add more as needed to handle various contexts.
                _ => throw ClientInducedException.MessageOnly("Unknown user context.")
            };


            //Find and remove any previous session tokens.
            var existingSessionTokensForSource =  await db.UserSessionTokens.Where(t => t.AssignedToUserId == userId && t.Source == userContext).ToListAsync();

            if(existingSessionTokensForSource.Count > 0)
            {
                db.RemoveRange(existingSessionTokensForSource);
            }

            var newSessionTokenEntry = new UserSessionTokenEntry
            {
                UserSessionTokenId = DbUtils.GenerateUuid(),
                AssignedToUserId = userId,
                SessionToken = userSessionToken,
                DateCreated = DateTime.UtcNow,
                IsRevoked = false,
                DateExpired = new DateTime(userSessionTokenExpiration),
                Source = userContext
            };

            await db.UserSessionTokens.AddAsync(newSessionTokenEntry);

            //update the user's last login time
            var userEntry = db.Users.Single(u => u.UserId == userId);
            userEntry.LastLoginTime = DateTime.Now;
            db.Update(userEntry);

            await db.SaveChangesAsync();
            var userProfile = GetUserBasicInfo(userId);
            //Return the result
            var res = new AdminUserLoginResponse()
            {
                AccessToken = userSessionToken,
                AccessTokenExpiration = userSessionTokenExpiration,
                UserRefreshToken = userRefreshToken,
                UserProfile = userProfile
            };

            return res;                       
        }

        private static UserBasicInfo GetUserBasicInfo(string userId)
        {
            using var db = new AppDbContext();
            var foundUser = db.Users.Single(u => u.UserId == userId);
            return new UserBasicInfo
            {
                User = foundUser
            };
        }
    }
}