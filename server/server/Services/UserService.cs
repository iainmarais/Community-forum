/*
User service.cs - responsible for:
    logins
    user registrations
    user administration
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
using RestApiServer.Utils;

namespace RestApiServer.Services
{
    public class UserService
    {
        public static async Task<UserLoginResponse> Login(string inputUserIdentifier, string password, string src)
        {
            string userContextIdentifier = "";   
            string username = "";
            string emailAddress = "";
            UserEntry? user = null;
            
            var userIdentifier = inputUserIdentifier.Trim().ToLower().Split(':');
            if(userIdentifier.Length > 1)
            {
                //What context is this user logging in to?
                userContextIdentifier = userIdentifier[1];
            }

            if(IsValidEmailAddress(userIdentifier[0]))
            {
                emailAddress = userIdentifier[0];
                if(!IsValidEmail(emailAddress))
                {
                    throw ClientInducedException.MessageOnly("Invalid email address provided. Please try again.");
                }
            }
            else
            {
                username = userIdentifier[0];
                if(!IsValidUsername(username))
                {
                    throw ClientInducedException.MessageOnly("Invalid username provided. Please try again.");
                }
            }
            using var db = new AppDbContext();

            //Handle the user context
            if(string.IsNullOrEmpty(userContextIdentifier))
            {
                user = await (from u in db.Users 
                                join r in db.Roles on u.RoleId equals r.RoleId
                                where u.Username == username 
                                || u.EmailAddress == emailAddress 
                                select u).SingleOrDefaultAsync();
                if(user == null)
                {
                    throw ClientInducedException.MessageOnly("Invalid username or email address provided, or user does not exist.");
                }

                if(!AuthUtils.VerifyPassword(password, user.HashedPassword))
                {
                    throw ClientInducedException.MessageOnly("Invalid password entered.");
                }
                return await LoginSuccessResponse(db, user.UserId, src);
            }
            else
            {
                //Since there are no alternate contexts the user can log into, throw an exception if this is attempted.
                throw ClientInducedException.MessageOnly("Alternative user context not implemented yet.");
            }
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

        public static async Task<UserBasicInfo> Register(string username, string roleType, string emailAddress, string cleartextpassword, string retypePassword) 
        {   
            //Someone's obviously fucked something up here. The question is have they unfucked it yet? :D
            if(username == null || emailAddress == null)
            {
                throw ClientInducedException.MessageOnly("Username and email address are required");
            }
            if(!IsValidUsername(username))
            {
                throw ClientInducedException.MessageOnly("Invalid username");
            }
            if(!IsValidEmail(emailAddress))
            {
                throw ClientInducedException.MessageOnly("Invalid email address");
            }
            if(cleartextpassword != retypePassword)
            {
                throw ClientInducedException.MessageOnly("Passwords do not match.");
            }
            if(roleType == null)
            {
                //User did not specify a role, assume user (RoleId = 1)
                roleType = "User";
            }

            using var db = new AppDbContext();
            //Check if there is already a user present with this username or email address.
            if(await db.Users.AnyAsync(u => u.Username == username || u.EmailAddress == emailAddress))
            {
                throw ClientInducedException.MessageOnly("User already exists");
            }

            var salt = AuthUtils.GenerateSalt();
            var user = new UserEntry
            {
                UserId = DbUtils.GenerateUuid(),
                AdminUserId = DbUtils.GenerateUuid(),
                ForumUserId = DbUtils.GenerateUuid(),
                RoleId = db.Roles.Single(r => r.RoleType.ToString().Contains(roleType)).RoleId,
                Username = username,
                EmailAddress = emailAddress,
                HashedPassword = AuthUtils.HashPassword(cleartextpassword, salt),
                //Todo: Create an entry or list entry to hold the user's role or permissions here.
            };
            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();
            return new UserBasicInfo()
            {
                User = user
            };
        }

        public static async Task<UserLoginResponse> LoginSuccessResponse(AppDbContext db, string userId, string src)
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

            var permissions = await (from userPermission in db.UserPermissions
                                    join systemPermission in db.SystemPermissions
                                    on userPermission.SystemPermissionId equals systemPermission.SystemPermissionId
                                    where userPermission.UserId == userId
                                    select systemPermission.Permission).Distinct().ToListAsync();
            var user = new
            {
                User = userResult.User,
                Permissions = permissions,
                Role = userResult.Role
            };
            var existingRefreshTokensForSource = await db.UserRefreshTokens.Where(t => t.UserId == userId && t.Source == src).ToListAsync();
            db.RemoveRange(existingRefreshTokensForSource);
            //Create the user refresh token.
            var(userRefreshToken, userRefreshTokenExpiration) = AuthUtils.GenerateRefreshToken();

            var newRefreshToken = new UserRefreshTokenEntry
            {
                UserRefreshTokenId = DbUtils.GenerateUuid(),
                UserId = userId,
                RefreshToken = userRefreshToken,
                RefreshTokenExpirationDate = userRefreshTokenExpiration,
                Source = src
            };
            await db.UserRefreshTokens.AddAsync(newRefreshToken);
            //Create the access token.
            var(accessToken, accessTokenExpiration) = AuthUtils.GenerateForumUserAccessToken(userId, userResult.User.ForumUserId, user.Permissions);

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
                RefreshToken = userRefreshToken,
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

        private static bool IsValidEmailAddress(string userIdentifier)
        {
            try 
            {
                var mailAddress = new MailAddress(userIdentifier);
                return mailAddress.Address == userIdentifier;
            }
            catch 
            {
                return false;
            }
        }
    }
}