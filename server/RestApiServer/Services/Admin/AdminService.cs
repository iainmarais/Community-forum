using Microsoft.EntityFrameworkCore;
using RestApiServer.Core.Errorhandler;
using RestApiServer.Db;
using RestApiServer.Dto.Admin;
using RestApiServer.Dto.App;
using RestApiServer.Enums;
using RestApiServer.Utils;

namespace RestApiServer.Services.Admin
{
    public class AdminService
    {
        public static async Task AssignUserRoleAsync(string userId, AssignRoleRequest request)
        {
            using var db = new AppDbContext();
            //Find the user in the database
            var foundUser = db.Users.Single(u => u.UserId == userId);
            var foundUserRole = db.Roles.Single(r => r.RoleId == foundUser.RoleId);
            
            //Replace this with a permissions-based check at a later point.
            if(foundUserRole.RoleType != RoleType.Admin)
            {
                //User is not an administrator
                throw ClientInducedException.MessageOnly("User is not an administrator.");
            }

            //Assign the selected user the selected role.
            var userToModify = db.Users.Single(u => u.UserId == request.SelectedUserId);
            var RoleToAdd = db.Roles.Single(r => r.RoleType.ToString() == request.SelectedRoleType);
            userToModify.RoleId = RoleToAdd.RoleId;

            //Update the database with the modified user details.
            db.Update(userToModify);
            await db.SaveChangesAsync();
        }

        public static async Task<List<RoleBasicInfo>> GetRolesAsync(string userId)
        {
            using var db = new AppDbContext();
            //Find the user in the database
            var foundUser = db.Users.Single(u => u.UserId == userId);
            var foundUserRole = db.Roles.Single(r => r.RoleId == foundUser.RoleId);
            
            //Replace this with a permissions-based check at a later point.
            if(foundUserRole.RoleType != RoleType.Admin)
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
            var foundUser = db.Users.Single(u => u.UserId == userId);
            var foundUserRole = db.Roles.Single(r => r.RoleId == foundUser.RoleId);
            
            //Replace this with a permissions-based check at a later point.
            if(foundUserRole.RoleType != RoleType.Admin)
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