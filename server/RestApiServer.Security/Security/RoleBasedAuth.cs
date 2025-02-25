using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using RestApiServer.Db;
using Serilog;

namespace RestApiServer.Security
{
    public static class RoleBasedAuth
    {
        public static AuthorizationOptions Configure(AuthorizationOptions options)
        {   
            try
            {
                //Create these mappings dynamically rather than hardcoding them - kills boilerplate.
                using var db = new AppDbContext();

                var permissions = db.RolePermissions
                    .Include(rp => rp.Role)
                    .Include(rp => rp.Permission)
                    .Where(rp => rp.IsAllowed == true)
                    .ToList();

                foreach(var permission in permissions)
                {
                    Log.Information($"Adding permission {permission.Permission.PermissionName} to role {permission.Role.RoleName}");
                    options.AddPolicy(permission.Permission.PermissionName, policy => policy.RequireRole(permission.Role.RoleName));
                }
            }

            catch (Exception ex)
            {
                Log.Error($"Something went horribly wrong configuring role-based authorisation.\n {ex.Message}", ex);
            }

            return options;
        }
    }
}

/*
What I see as a table for establishing role-permission mappings:
************************************************************************************************************
RolePermissionId (Guid, primary key)        RoleId (Guid, foreign key, ref RoleId on Roles table)     PermissionId (Guid, foreign key, ref PermissionId on Permissions table)       IsAllowed (bool - 0 (false) or 1 (true))
************************************************************************************************************
*/