/*
User service.cs - responsible for:
    logins
    user registrations
    user administration
    refreshing login states using user refresh tokens
*/

using System.Net.Mail;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using RestApiServer.Core.CacheManagement;
using RestApiServer.Core.Errorhandler;
using RestApiServer.Db;
using RestApiServer.Db.Users;
using RestApiServer.Dto.App;
using RestApiServer.Dto.Forum;
using RestApiServer.Dto.Login;
using RestApiServer.Enums;
using RestApiServer.Utils;

namespace RestApiServer.Services
{
    public class UserService
    {
        public static async Task<UserLoginResponse> Login(UserLoginRequest req)
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
                //Handle the user context now.
                switch(req.UserContext)
                {
                    case "forum":
                    case "admin":
                        return await LoginSuccessResponse(db, foundUser.UserId, foundUser.RoleId, req.UserContext);
                    case "chat":
                        throw ClientInducedException.MessageOnly("Chat context not yet implemented.");
                    default:
                        throw ClientInducedException.MessageOnly("Invalid user context.");
                }
            }
            //Invalid username or email address.
            else
            {
                throw ClientInducedException.MessageOnly("Please check your login credentials.")   ;
            }
        }

        //Find the user asynchronously
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

        public static UserBasicInfo GetUserBasicInfo(string userId)
        {
            var cachedData = ShortTermCache.TryGetFromCache<UserBasicInfo>(userId);
            if (cachedData != null)
            {
                return cachedData;
            }
            else
            {
                using var db = new AppDbContext();
                var user = db.Users.Single(u => u.UserId == userId);
                var userBasicInfo = new UserBasicInfo()
                {
                    User = user
                };
                ShortTermCache.AddToCache(userId, userBasicInfo);
                return userBasicInfo;
            }
        }
        public static async Task<UserBasicInfo> GetUserBasicInfoAsync(string userId)
        {        
            using var db = new AppDbContext();
            var user = await db.Users.SingleAsync(u => u.UserId == userId);
            return new UserBasicInfo()
            {
                User = user
            };
        }

        public static async Task<UserBasicInfo> Register(UserRegistrationRequest req) 
        {   
            if(req.Username == null || req.EmailAddress == null || req.Password == null || req.RetypePassword == null)
            {
                throw ClientInducedException.MessageOnly("Username and email address are required");
            }
            if(!IsValidUsername(req.Username))
            {
                throw ClientInducedException.MessageOnly("Invalid username");
            }
            if(!IsValidEmail(req.EmailAddress))
            {
                throw ClientInducedException.MessageOnly("Invalid email address");
            }
            if(req.Password != req.RetypePassword)
            {
                throw ClientInducedException.MessageOnly("Passwords do not match.");
            }
            if(req.RoleType == null)
            {
                //User did not specify a role, assume user
                req.RoleType = "User";
            }

            using var db = new AppDbContext();
            //Check if there is already a user present with this username or email address.
            if(await db.Users.AnyAsync(u => u.Username == req.Username || u.EmailAddress == req.EmailAddress))
            {
                throw ClientInducedException.MessageOnly("User already exists");
            }

            var salt = AuthUtils.GenerateSalt();
            
            if(!Enum.TryParse(req.RoleType, out RoleType parsedRoleType))
            {
                throw ClientInducedException.MessageOnly("Invalid role type");
            }
            var user = new UserEntry
            {
                UserId = DbUtils.GenerateUuid(),
                AdminUserId = DbUtils.GenerateUuid(),
                ForumUserId = DbUtils.GenerateUuid(),
                RoleId = db.Roles.Single(r => r.RoleType == parsedRoleType).RoleId,
                Username = req.Username,
                EmailAddress = req.EmailAddress,
                HashedPassword = AuthUtils.HashPassword(req.Password, salt),
                //Todo: Create an entry or list entry to hold the user's role or permissions here.
            };
            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();
            return new UserBasicInfo()
            {
                User = user
            };
        }

        public static async Task<UserRefreshResponse> RefreshUserSessionAsync(string userId, UserRefreshRequest req)
        {
            using var db = new AppDbContext();

            //Find the user in the db given the incoming user Id:
            var userResult = await (from u in db.Users 
                                    where u.UserId == userId 
                                    join role in db.Roles on u.RoleId equals role.RoleId
                                    select new 
                                    {
                                        User = u,
                                        Role = role
                                    }).SingleAsync();

            //SingleOrDefaultAsync will find a result or return a functionally null yet valid result.
            if(userResult == null || userResult.User.UserId != userId)
            {
                throw ClientInducedException.MessageOnly("User not found in database.");
            }
            //Else, continue to check our token.
            var existingRefreshToken = await db.UserRefreshTokens.SingleAsync(urt => urt.AssignedToUserId == userId && urt.Source == req.UserContext);

            if(existingRefreshToken.RefreshTokenExpirationDate < DateTime.UtcNow)
            {
                throw ClientInducedException.MessageOnly("Refresh token has expired.");
            }

            //If this token was revoked, e.g. by logging off, we can't proceed any further.
            if(existingRefreshToken.IsRevoked)
            {
                throw ClientInducedException.MessageOnly("User refresh token has been revoked.");
            }

            var permissions = await (from up in db.UserPermissions
                                    join sp in db.SystemPermissions
                                    on up.SystemPermissionId equals sp.SystemPermissionId
                                    where up.UserId == userId
                                    select sp.SystemPermissionType)
                                    .Distinct().ToListAsync();

            //Permissions for roles
            var rolePermissions = await (from rp in db.RolePermissions
                                    join p in db.Permissions
                                    on rp.PermissionId equals p.PermissionId
                                    where rp.RoleId == userResult.Role.RoleId
                                    select p.PermissionType)
                                    .Distinct().ToListAsync();                                    
            var user = new
            {
                userResult.User,
                Permissions = permissions,
                RolePermissions = rolePermissions,
                userResult.Role
            };
            var existingRefreshTokensForSource = await db.UserRefreshTokens.Where(t => t.AssignedToUserId == userId && t.Source == req.UserContext).ToListAsync();

            db.RemoveRange(existingRefreshTokensForSource);
            //Create the user refresh token.
            var(userRefreshToken, userRefreshTokenExpiration) = AuthUtils.GenerateRefreshToken();

            var newRefreshToken = new UserRefreshTokenEntry
            {
                UserRefreshTokenId = DbUtils.GenerateUuid(),
                AssignedToUserId = userId,
                RefreshToken = userRefreshToken,
                RefreshTokenExpirationDate = userRefreshTokenExpiration,
                Source = req.UserContext ?? "Unknown",
                IsRevoked = false
            };
            await db.UserRefreshTokens.AddAsync(newRefreshToken);
            //Now, one can create the new access and refresh tokens.
            var (newAccessToken, newAccessTokenExpiration) = req.UserContext switch
            {
                "admin" => AuthUtils.GenerateAdminUserAccessToken(userId, userResult.User.AdminUserId, permissions, new(){ userResult.Role.RoleType }),
                "forum" => AuthUtils.GenerateForumUserAccessToken(userId, userResult.User.ForumUserId, permissions, new(){ userResult.Role.RoleType }),
                //Add more as needed to handle various contexts.
                _ => throw ClientInducedException.MessageOnly("Unknown user context.")
            };

            var userProfile = GetUserBasicInfo(userId);

            var res = new UserRefreshResponse
            {
                NewAccessToken = newAccessToken,
                NewAccessTokenExpiration = newAccessTokenExpiration,
                RefreshToken = userRefreshToken,
                UserProfile = userProfile
            };

            return res;
        }

        public static async Task<UserLoginResponse> LoginSuccessResponse(AppDbContext db, string userId, string roleId, string userContext)
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
                                    join sp in db.SystemPermissions
                                    on up.SystemPermissionId equals sp.SystemPermissionId
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

            var (accessToken, accessTokenExpiration) = userContext switch
            {
                "admin" => AuthUtils.GenerateAdminUserAccessToken(userId, userResult.User.AdminUserId, permissions, new(){ userResult.Role.RoleType }),
                "forum" => AuthUtils.GenerateForumUserAccessToken(userId, userResult.User.ForumUserId, permissions, new(){ userResult.Role.RoleType }),
                //Add more as needed to handle various contexts.
                _ => throw ClientInducedException.MessageOnly("Unknown user context.")
            };

            //update the user's last login time
            var userEntry = db.Users.Single(u => u.UserId == userId);
            userEntry.LastLoginTime = DateTime.Now;
            db.Update(userEntry);

            await db.SaveChangesAsync();
            var userProfile = GetUserBasicInfo(userId);
            //Return the result
            var res = new UserLoginResponse()
            {
                AccessToken = accessToken,
                AccessTokenExpiration = accessTokenExpiration,
                UserRefreshToken = userRefreshToken,
                UserProfile = userProfile
            };

            return res;
        }

        public static async Task<UserBasicInfo> UpdateUserProfileAsync(UpdateUserProfileRequest request)
        {
            using var db = new AppDbContext();
            var user = await db.Users.SingleAsync(u => u.UserId == request.UserId);
            user.UserFirstname = request.UserFirstname;
            user.UserLastname = request.UserLastname;
            user.UserProfileImageBase64 = request.UserProfileImageBase64;
            await db.SaveChangesAsync();
            //Todo: Add user profile update logic here.

            var res = GetUserBasicInfo(user.UserId);
            return res;
        }

        private static bool IsValidEmail(string email)
        {
            // Add email validation logic here
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        private static bool IsValidUsername(string username)
        {
            // Add username validation logic here, e.g., length and allowed characters
            return !string.IsNullOrEmpty(username) && username.Length >= 3 && username.Length <= 50;
        }
    }
}