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
                user = await db.Users.SingleOrDefaultAsync( u => u.Username.ToLower() == username || u.EmailAddress.ToLower() == emailAddress);
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
                var userBasicInfo = new UserBasicInfo(user);
                ShortTermCache.AddToCache(userId, userBasicInfo);
                return userBasicInfo;
            }
        }
        public static async Task<UserBasicInfo> GetUserBasicInfoAsync(string userId)
        {        
            using var db = new AppDbContext();
            var user = await db.Users.SingleAsync(u => u.UserId == userId);
            return new UserBasicInfo(user);
        }

        public static async Task<UserBasicInfo> Register(string username, string emailAddress, string cleartextpassword, string retypePassword) 
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
                Username = username,
                EmailAddress = emailAddress,
                HashedPassword = AuthUtils.HashPassword(cleartextpassword, salt),
                //Todo: Create an entry or list entry to hold the user's role or permissions here.
            };
            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();
            return new UserBasicInfo(user);
        }

        public static async Task<UserLoginResponse> LoginSuccessResponse(AppDbContext db, string userId, string src)
        {
            //Works with MySql.
            // var userResult = await (from user in db.Users 
            //                 where user.UserId == userId 
            //                 select new 
            //                 {
            //                     User = user,
            //                     Permissions = (
            //                         from userPermission in db.UserPermissions
            //                         join systemPermission in db.SystemPermissions
            //                         on userPermission.SystemPermissionId equals systemPermission.SystemPermissionId
            //                         where userPermission.UserId == userId
            //                         select systemPermission.Permission
            //                     ).Distinct().ToList()
            //                 }).SingleAsync();

            //When using MariaDb, use this approach:
            //To be brutally honest I prefer doing it like this - it makes more sense from a logical perspective as its a bit more procedure-oriented.
            var user = await db.Users.SingleAsync(u => u.UserId == userId) ?? throw new Exception("User not found");
            var permissions = await (from userPermission in db.UserPermissions
                                    join systemPermission in db.SystemPermissions
                                    on userPermission.SystemPermissionId equals systemPermission.SystemPermissionId
                                    where userPermission.UserId == userId
                                    select systemPermission.Permission).Distinct().ToListAsync();
            var role = await db.Roles.SingleAsync(r => r.RoleId == user.RoleId) ?? throw new Exception("Role not found");
            var userResult = new
            {
                User = user,
                Permissions = permissions,
                Role = role
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
            var(accessToken, accessTokenExpiration) = AuthUtils.GenerateUserAccessToken(userId, userResult.Permissions);

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