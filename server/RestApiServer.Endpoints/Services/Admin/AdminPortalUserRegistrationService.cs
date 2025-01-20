using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using RestApiServer.Common.Services;
using RestApiServer.CommonEnums;
using RestApiServer.Core.Errorhandler;
using RestApiServer.Database.Utils;
using RestApiServer.Db;
using RestApiServer.Db.Users;
using RestApiServer.Dto.Admin;
using RestApiServer.Dto.App;

namespace RestApiServer.Endpoints.Services.Admin
{
    public class AdminPortalUserRegistrationService
    {
        public static async Task<UserBasicInfo> RegisterUser(AdminPortalUserRegistrationRequest req)
        {
            using var db = new AppDbContext();
            if (req.Username == null || req.EmailAddress == null || req.Password == null || req.RetypePassword == null)
            {
                throw ClientInducedException.MessageOnly("Username and email address are required");
            }
            if (!IsValidUsername(req.Username))
            {
                throw ClientInducedException.MessageOnly("Invalid username");
            }
            if (!IsValidEmail(req.EmailAddress))
            {
                throw ClientInducedException.MessageOnly("Invalid email address");
            }
            if (req.Password != req.RetypePassword)
            {
                throw ClientInducedException.MessageOnly("Passwords do not match.");
            }
            if (req.RoleType == null)
            {
                //User did not specify a role, assume user
                req.RoleType = "User";
            }

            if (await db.Users.AnyAsync(u => u.Username == req.Username || u.EmailAddress == req.EmailAddress))
            {
                throw ClientInducedException.MessageOnly("User already exists");
            }

            var salt = AuthService.GenerateSalt();

            if (!Enum.TryParse(req.RoleType, out RoleType parsedRoleType))
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
                HashedPassword = AuthService.HashPassword(req.Password, salt),
                RegistrationTime = DateTime.UtcNow,
                //Todo: Create an entry or list entry to hold the user's role or permissions here.
            };

            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();
            return new UserBasicInfo()
            {
                User = user
            };
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