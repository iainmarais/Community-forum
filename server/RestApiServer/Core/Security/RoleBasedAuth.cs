namespace RestApiServer.Security
{
    public static class RoleBasedAuth
    {
        public static void Configure(Microsoft.AspNetCore.Authorization.AuthorizationOptions options)
        {
            //Establish new policies based on roles. This is a more modular approach to hard-coding policies in the endpoints.
            options.AddPolicy("EditUsersPolicy", policy => policy.RequireRole("Admin", "Manager"));
            options.AddPolicy("CreateUsersPolicy", policy => policy.RequireRole("Admin"));
            //Todo: Expand these policies. Will need to create the permissions table and a role-permission many-to-many relationship in the DB.
        }
    }
}

/*
What I see as a table for establishing role-permission mappings:
************************************************************************************************************
RolePermissionId (Guid, primary key)        RoleId (Guid, foreign key, ref RoleId on Roles table)     PermissionId (Guid, foreign key, ref PermissionId on Permissions table)       IsAllowed (bool - 0 (false) or 1 (true))
************************************************************************************************************
*/