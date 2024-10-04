using RestApiServer.Db;
using RestApiServer.Enums;
using Microsoft.AspNetCore.Authorization;

namespace RestApiServer.Core.Security
{

    public static class UserAuthorisationPolicies
    {
        private static List<string> ValidAdminUserClaimsRequirements = new(){"UserId", "AdminUserId"};
        private static List<string> ValidForumUserClaimsRequirements = new(){"UserId", "ForumUserId"};
        //For chat. Todo: Create claims handler for this and extend the user datamodel.
        private static List<string> ValidChatUserClaimsRequirements = new(){ "UserId", "ChatUserId"};
        private static string UserClaimRequirement = "UserId";
        public static AuthorizationOptions Configure(AuthorizationOptions options)
        {
            //User related policies
            options.AddPolicy(CreateUsersPolicy, policy => policy.RequireClaim(SystemPermissionType.Users_Create.ToString(),ValidAdminUserClaimsRequirements));
            options.AddPolicy(EditUsersPolicy, policy => policy.RequireClaim(SystemPermissionType.Users_Edit.ToString(),ValidAdminUserClaimsRequirements));
            options.AddPolicy(DeleteUsersPolicy, policy => policy.RequireClaim(SystemPermissionType.Users_Delete.ToString(),ValidAdminUserClaimsRequirements));
            options.AddPolicy(ChangeUserRolePolicy, policy => policy.RequireClaim(SystemPermissionType.Users_ChangeRoles.ToString(),ValidAdminUserClaimsRequirements));
            options.AddPolicy(ChangeUserPasswordPolicy, policy => policy.RequireClaim(SystemPermissionType.Users_ChangePassword.ToString(),ValidAdminUserClaimsRequirements));
            options.AddPolicy(BanUserPolicy, policy => policy.RequireClaim(SystemPermissionType.Users_BanUser.ToString(),ValidAdminUserClaimsRequirements));
            //Role related policies
            options.AddPolicy(CreateRolesPolicy, policy => policy.RequireClaim(SystemPermissionType.Roles_Create.ToString(),ValidAdminUserClaimsRequirements));
            options.AddPolicy(EditRolesPolicy, policy => policy.RequireClaim(SystemPermissionType.Roles_Edit.ToString(),ValidAdminUserClaimsRequirements));
            options.AddPolicy(DeleteRolesPolicy, policy => policy.RequireClaim(SystemPermissionType.Roles_Delete.ToString(),ValidAdminUserClaimsRequirements));
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
            //Chat related policies
            options.AddPolicy(CreateChatPolicy, policy => policy.RequireClaim(SystemPermissionType.Chat_Create.ToString()));
            options.AddPolicy(EditChatPolicy, policy => policy.RequireClaim(SystemPermissionType.Chat_Edit.ToString()));
            options.AddPolicy(DeleteChatPolicy, policy => policy.RequireClaim(SystemPermissionType.Chat_Delete.ToString()));
            options.AddPolicy(ChatPostImagesPolicy, policy => policy.RequireClaim(SystemPermissionType.Chat_PostImage.ToString()));
            options.AddPolicy(CreateGroupChatPolicy, policy => policy.RequireClaim(SystemPermissionType.Chat_CreateGroup.ToString()));
            options.AddPolicy(EditGroupChatPolicy, policy => policy.RequireClaim(SystemPermissionType.Chat_EditGroup.ToString()));
            options.AddPolicy(DeleteGroupChatPolicy, policy => policy.RequireClaim(SystemPermissionType.Chat_DeleteGroup.ToString()));
            options.AddPolicy(JoinGroupChatPolicy, policy => policy.RequireClaim(SystemPermissionType.Chat_JoinGroup.ToString()));
            //Gallery
            options.AddPolicy(GalleryImageUploadPolicy, policy => policy.RequireClaim(SystemPermissionType.Gallery_UploadImage.ToString()));
            options.AddPolicy(GalleryImageDeletePolicy, policy => policy.RequireClaim(SystemPermissionType.Gallery_DeleteImage.ToString()));
            options.AddPolicy(GalleryImageEditPolicy, policy => policy.RequireClaim(SystemPermissionType.Gallery_EditImage.ToString()));
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
        //Chat 
        public const string CreateChatPolicy = "CreateChatPolicy";
        public const string EditChatPolicy = "EditChatPolicy";
        public const string DeleteChatPolicy = "DeleteChatPolicy";
        public const string ChatPostImagesPolicy = "ChatPostImagesPolicy";
        public const string CreateGroupChatPolicy = "CreateGroupChatPolicy";
        public const string EditGroupChatPolicy = "EditGroupChatPolicy";
        public const string DeleteGroupChatPolicy = "DeleteGroupChatPolicy";
        public const string JoinGroupChatPolicy = "JoinGroupChatPolicy";
        public const string GalleryImageUploadPolicy = "GalleryImageUploadPolicy";
        public const string GalleryImageDeletePolicy = "GalleryImageDeletePolicy";
        public const string GalleryImageEditPolicy = "GalleryImageEditPolicy";
    }
}