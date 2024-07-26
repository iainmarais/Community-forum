using RestApiServer.Db;
using RestApiServer.Enums;

namespace RestApiServer.Core.Security
{
    public static class UserAuthorisationPolicies
    {
        public static Microsoft.AspNetCore.Authorization.AuthorizationOptions Configure(Microsoft.AspNetCore.Authorization.AuthorizationOptions options)
        {
            //User related policies
            options.AddPolicy(CreateUsersPolicy, policy => policy.RequireClaim(SystemPermissionType.Users_Create.ToString()));
            options.AddPolicy(EditUsersPolicy, policy => policy.RequireClaim(SystemPermissionType.Users_Edit.ToString()));
            options.AddPolicy(DeleteUsersPolicy, policy => policy.RequireClaim(SystemPermissionType.Users_Delete.ToString()));
            options.AddPolicy(ChangeUserRolePolicy, policy => policy.RequireClaim(SystemPermissionType.Users_ChangeRoles.ToString()));
            options.AddPolicy(ChangeUserPasswordPolicy, policy => policy.RequireClaim(SystemPermissionType.Users_ChangePassword.ToString()));
            options.AddPolicy(BanUserPolicy, policy => policy.RequireClaim(SystemPermissionType.Users_BanUser.ToString()));
            //Role related policies
            options.AddPolicy(CreateRolesPolicy, policy => policy.RequireClaim(SystemPermissionType.Roles_Create.ToString()));
            options.AddPolicy(EditRolesPolicy, policy => policy.RequireClaim(SystemPermissionType.Roles_Edit.ToString()));
            options.AddPolicy(DeleteRolesPolicy, policy => policy.RequireClaim(SystemPermissionType.Roles_Delete.ToString()));
            //Threads
            options.AddPolicy(CreateThreadsPolicy, policy => policy.RequireClaim(SystemPermissionType.Threads_Create.ToString()));
            options.AddPolicy(EditThreadsPolicy, policy => policy.RequireClaim(SystemPermissionType.Threads_Edit.ToString()));
            options.AddPolicy(DeleteThreadsPolicy, policy => policy.RequireClaim(SystemPermissionType.Threads_Delete.ToString()));
            options.AddPolicy(LockThreadsPolicy, policy => policy.RequireClaim(SystemPermissionType.Threads_Lock.ToString()));
            options.AddPolicy(UnlockThreadsPolicy, policy => policy.RequireClaim(SystemPermissionType.Threads_Unlock.ToString()));
            //Messages
            options.AddPolicy(CreatePostsPolicy, policy => policy.RequireClaim(SystemPermissionType.Posts_Create.ToString()));
            options.AddPolicy(EditPostsPolicy, policy => policy.RequireClaim(SystemPermissionType.Posts_Edit.ToString()));
            options.AddPolicy(DeletePostsPolicy, policy => policy.RequireClaim(SystemPermissionType.Posts_Delete.ToString()));
            options.AddPolicy(UpdatePostsPolicy, policy => policy.RequireClaim(SystemPermissionType.Posts_Update.ToString()));
            options.AddPolicy(PostImagesPolicy, policy => policy.RequireClaim(SystemPermissionType.Posts_PostImage.ToString()));
            options.AddPolicy(PostRepliesPolicy, policy => policy.RequireClaim(SystemPermissionType.Posts_PostReply.ToString()));
            //Topic related policies
            options.AddPolicy(CreateTopicsPolicy, policy => policy.RequireClaim(SystemPermissionType.Topics_Create.ToString()));
            options.AddPolicy(EditTopicsPolicy, policy => policy.RequireClaim(SystemPermissionType.Topics_Edit.ToString()));
            options.AddPolicy(DeleteTopicsPolicy, policy => policy.RequireClaim(SystemPermissionType.Topics_Delete.ToString()));
            
            return options; 
        }

        //User policies
        public const string CreateUsersPolicy = "CreateUsersPolicy";
        public const string EditUsersPolicy = "EditUsersPolicy";
        public const string DeleteUsersPolicy = "DeleteUsersPolicy";
        public const string ChangeUserRolePolicy = "ChangeUserRolePolicy";
        public const string ChangeUserPasswordPolicy = "ChangeUserPasswordPolicy";
        public const string BanUserPolicy = "ChangeUserEmailPolicy";
        //Role policies
        public const string CreateRolesPolicy = "CreateRolesPolicy";
        public const string EditRolesPolicy = "EditRolesPolicy";
        public const string DeleteRolesPolicy = "DeleteRolesPolicy";
        //Threads
        public const string CreateThreadsPolicy = "CreateThreadsPolicy";
        public const string EditThreadsPolicy = "EditThreadsPolicy";
        public const string DeleteThreadsPolicy = "DeleteThreadsPolicy";
        public const string LockThreadsPolicy = "LockThreadsPolicy";
        public const string UnlockThreadsPolicy = "UnlockThreadsPolicy";
        //Messages
        public const string CreatePostsPolicy = "CreatePostsPolicy";
        public const string EditPostsPolicy = "EditPostsPolicy";
        public const string DeletePostsPolicy = "DeletePostsPolicy";
        public const string UpdatePostsPolicy = "UpdatePostsPolicy";
        public const string PostImagesPolicy = "PostImagesPolicy";
        public const string PostRepliesPolicy = "PostRepliesPolicy";
        //Topics
        public const string CreateTopicsPolicy = "CreateTopicsPolicy";
        public const string EditTopicsPolicy = "EditTopicsPolicy";
        public const string DeleteTopicsPolicy = "DeleteTopicsPolicy";
    }
}