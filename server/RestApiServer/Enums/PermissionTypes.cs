namespace RestApiServer.Enums
{
    public enum SystemPermissionType
    {
        Users_Create,
        Users_Edit,
        Users_Delete,
        Users_ChangeRoles,
        Users_ChangePassword,
        Users_BanUser,
        Roles_Create,
        Roles_Edit,
        Roles_Delete,
        Threads_Create,
        Threads_Edit,
        Threads_Delete,
        Threads_Lock,
        Threads_Unlock,
        Posts_Create,
        Posts_Edit,
        Posts_Delete,
        Posts_Update,
        Posts_PostImage,
        Posts_PostReply,
        Topics_Create,
        Topics_Edit,
        Topics_Delete,
        Chat_Create,
        Chat_Edit,
        Chat_Delete,
        Chat_PostImage,
        Chat_CreateGroup,
        Chat_EditGroup,
        Chat_DeleteGroup,
        Chat_JoinGroup,
        Gallery_UploadImage,
        Gallery_DeleteImage,
        Gallery_EditImage
    }

    //for role-based authorisation.
    /*
    Roles:
        (1, 'Administrator', 'Admin', 'Full access to manage the forum'),
        (2, 'Moderator', 'Moderator', 'Monitors activities and enforces rules'),
        (3, 'Content Creator', 'Creator', 'Produces high-quality content'),
        (4, 'Community Manager', 'Manager', 'Engages the community and organizes events'),
        (5, 'Expert', 'Expert', 'Recognized for expertise in specific areas'),
        (6, 'User', 'User', 'Standard members with posting privileges'),
        (7, 'Guest', 'Guest', 'Non-registered users with limited access'),
        (8, 'Banned', 'Banned', 'Users banned from the forum'),
        (9, 'Support', 'Support', 'Assists users with forum-related issues'),
        (10, 'Junior Moderator', 'JuniorModerator', 'Moderators-in-training with limited permissions'),
        (11, 'Sponsor', 'Sponsor', 'Users/entities sponsoring the forum');
    */ 
    public enum PermissionType 
    {
        //User permissions

        //Admin permissions

        //Manager permissions

        //Creator permissions

        //Moderator permissions
    }
}
