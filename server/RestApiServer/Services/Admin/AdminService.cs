using Microsoft.EntityFrameworkCore;
using RestApiServer.Core.Errorhandler;
using RestApiServer.Db;
using RestApiServer.Db.Users;
using RestApiServer.Dto.Admin;
using RestApiServer.Dto.App;
using RestApiServer.CommonEnums;
using Serilog;
using RestApiServer.Common.Services;

namespace RestApiServer.Services.Admin
{
    public class AdminService
    {
        public static async Task CreateUserAsync(string adminUserId, CreateUserRequest request)
        {
            using var db = new AppDbContext();

            var foundUserRole = await db.Roles.SingleOrDefaultAsync(r => r.RoleType == RoleType.Admin);
            var foundUser = await db.Users.SingleOrDefaultAsync(u => u.UserId == adminUserId);

            if(foundUserRole == null)
            {
                throw ClientInducedException.MessageOnly("Admin role not found.");
            }

            if(foundUser == null)
            {
                throw ClientInducedException.MessageOnly("User not found.");
            }

            if (foundUser != null && foundUserRole != null)
            {
                if(foundUser.RoleId != foundUserRole.RoleId)
                {
                    throw ClientInducedException.MessageOnly("Request denied: User is not an administrator.");
                }
            }

            //First see if there is a user on the database with matching email address and/or username
            var existingUser = await db.Users.SingleOrDefaultAsync(u => u.Username == request.Username || u.EmailAddress == request.EmailAddress);

            if(existingUser != null)
            {
                //The user exists. Do nothing
                if(!string.IsNullOrEmpty(existingUser.Username))
                {
                    if(!string.IsNullOrEmpty(existingUser.EmailAddress))
                    {
                        Log.Information($"Found existing user: {existingUser.Username} with email address: {existingUser.EmailAddress}.");
                    }
                    else
                    {
                        Log.Information($"Found existing user: {existingUser.Username}");
                    }
                }
                return;
            }

            if(string.IsNullOrEmpty(request.RoleName))
            {
                throw ClientInducedException.MessageOnly("Role name not provided.");
            }

            if(request.Password != request.RetypePassword)
            {
                throw ClientInducedException.MessageOnly("Passwords do not match.");
            }
            var salt = AuthService.GenerateSalt();
            var role = await db.Roles.SingleOrDefaultAsync(r => r.RoleName == request.RoleName);

            if(role == null)
            {
                throw ClientInducedException.MessageOnly("Role not found.");
            }

            var userToCreate = new UserEntry()
            {
                UserId = Guid.NewGuid().ToString(),
                ForumUserId = Guid.NewGuid().ToString(),
                RoleId = role.RoleId,
                Username = request.Username,
                EmailAddress = request.EmailAddress,
                HashedPassword = AuthService.HashPassword(request.Password, salt)
            };

            await db.Users.AddAsync(userToCreate);
            await db.SaveChangesAsync();
        }
        //Update existing users
        public static async Task UpdateUserAsync(string adminUserId, UserBasicInfo userToUpdate)
        {
            using var db = new AppDbContext();
            //Find the admin user in the database
            var foundUser = await db.Users.SingleOrDefaultAsync(u => u.UserId == adminUserId);
            var foundUserRole = await db.Roles.SingleOrDefaultAsync(r => r.RoleId == foundUser!.RoleId);

            if(foundUserRole!.RoleType != RoleType.Admin)            
            {
                throw ClientInducedException.MessageOnly("User is not an administrator.");                
            }

            //At this point: Find the user entry matching the incoming user id and update it
            var userEntryToUpdate = await db.Users.SingleOrDefaultAsync(u => u.UserId == userToUpdate.User.UserId);
            
            if (userEntryToUpdate == null)
            {
                throw ClientInducedException.MessageOnly("User not found.");
            }

            if(!string.IsNullOrEmpty(userToUpdate.User.UserFirstname) || !string.IsNullOrEmpty(userToUpdate.User.UserLastname))    
            {
                userEntryToUpdate.UserFirstname = userToUpdate.User.UserFirstname;
                userEntryToUpdate.UserLastname = userToUpdate.User.UserLastname;
            }
            //Address and other contact info
            if(!string.IsNullOrEmpty(userToUpdate.User.Address) || !string.IsNullOrEmpty(userToUpdate.User.CityName) || !string.IsNullOrEmpty(userToUpdate.User.CountryName) || !string.IsNullOrEmpty(userToUpdate.User.PostalCode))
            {
                userEntryToUpdate.Address = userToUpdate.User.Address;
                userEntryToUpdate.CityName = userToUpdate.User.CityName;
                userEntryToUpdate.CountryName = userToUpdate.User.CountryName;
                userEntryToUpdate.PostalCode = userToUpdate.User.PostalCode;
            }
            //Add more as needed.
            if(!string.IsNullOrEmpty(userToUpdate.User.Gender))
            {
                userEntryToUpdate.Gender = userToUpdate.User.Gender;
            }
            
            //Update the user's details with those coming in
            db.Users.Update(userEntryToUpdate);
            await db.SaveChangesAsync();
        }

        public static async Task AssignUserRoleAsync(string adminUserId, AssignRoleRequest request)
        {
            using var db = new AppDbContext();
            //Find the user in the database
            var foundUser = await db.Users.SingleOrDefaultAsync(u => u.UserId == adminUserId);
            var foundUserRole = await db.Roles.SingleOrDefaultAsync(r => r.RoleId == foundUser!.RoleId);
            
            //Replace this with a permissions-based check at a later point.
            if(foundUserRole!.RoleType != RoleType.Admin)
            {
                //User is not an administrator
                throw ClientInducedException.MessageOnly("User is not an administrator.");
            }

            //Assign the selected user the selected role.
            var userToModify = await db.Users.SingleOrDefaultAsync(u => u.UserId == request.SelectedUserId);
            var RoleToAdd = await db.Roles.SingleOrDefaultAsync(r => r.RoleType.ToString() == request.SelectedRoleType);

            if(userToModify == null || RoleToAdd == null)
            {
                throw ClientInducedException.MessageOnly("User to modify not specified, or role to add not specified");
            }

            userToModify.RoleId = RoleToAdd.RoleId;

            //Update the database with the modified user details.
            db.Update(userToModify);
            await db.SaveChangesAsync();
        }

        public static async Task<List<RoleBasicInfo>> GetRolesAsync(string userId)
        {
            using var db = new AppDbContext();
            //Find the user in the database
            var foundUser = await db.Users.SingleOrDefaultAsync(u => u.UserId == userId);
            var foundUserRole = await db.Roles.SingleOrDefaultAsync(r => r.RoleId == foundUser!.RoleId);
            
            //Replace this with a permissions-based check at a later point.
            if(foundUserRole!.RoleType != RoleType.Admin)
            {
                //User is not an administrator
                throw ClientInducedException.MessageOnly("User is not an administrator.");
            }

            var roles = await db.Roles.Select(r => 
            new RoleBasicInfo()
                {
                Role = r
                }).ToListAsync();
            return roles;
        }
        //Users
        public static async Task<List<UserBasicInfo>> GetUsersAsync(string userId)
        {
                        using var db = new AppDbContext();
            //Find the user in the database
            var foundUser = await db.Users.SingleOrDefaultAsync(u => u.UserId == userId);
            var foundUserRole = await db.Roles.SingleOrDefaultAsync(r => r.RoleId == foundUser!.RoleId);
            
            //Replace this with a permissions-based check at a later point.
            if(foundUserRole!.RoleType != RoleType.Admin)
            {
                //User is not an administrator
                throw ClientInducedException.MessageOnly("User is not an administrator.");
            }

            var users = await db.Users.Select(u => 
            new UserBasicInfo()
                {
                    User = u
                }).ToListAsync();
            return users;
        }
    }
}