namespace RestApiServer.Enums
{
    public enum SystemPermissionType
    {
        Visibility,
        Access,
        Interactivity,
        General,
        Development,
        Testing,
        Production
        //Add more as needed.
    }

    public enum PermissionType 
    {
        Administrative,
        Content,
        General,
        Moderation
        //Add more as needed.
    }
    /*
    Keeping this SQL code here, for reference and when I need to use it.
        insert into permissions (PermissionId, PermissionName, PermissionType, Description) values 
        (perm_users_create, 'Users_Create', 'Administrative', 'Create new users'),
        (perm_users_edit, 'Users_Edit', 'Administrative', 'Edit existing users'),
        (perm_users_delete, 'Users_Delete', 'Administrative', 'Delete users'),
        (perm_users_change_roles, 'Users_ChangeRoles', 'Administrative', 'Change user roles'),
        (perm_users_change_password, 'Users_ChangePassword', 'Administrative', 'Change user password'),
        (perm_users_ban_user, 'Users_BanUser', 'Administrative', 'Ban user'),
        (perm_users_mute_user, 'Users_MuteUser', 'Administrative', 'Mute user'),
        (perm_users_warn_user, 'Users_WarnUser', 'Administrative', 'Warn user'),
        (perm_roles_create, 'Roles_Create', 'Administrative', 'Create new roles'),
        (perm_roles_edit, 'Roles_Edit', 'Administrative', 'Edit existing roles'),
        (perm_roles_delete, 'Roles_Delete', 'Administrative', 'Delete roles'),
        (perm_threads_create, 'Threads_Create', 'Content', 'Create new threads'),
        (perm_threads_edit, 'Threads_Edit', 'Content', 'Edit existing threads'),
        (perm_threads_delete, 'Threads_Delete', 'Content', 'Delete threads'),
        (perm_threads_lock, 'Threads_Lock', 'Content', 'Lock threads'),
        (perm_threads_unlock, 'Threads_Unlock', 'Content', 'Unlock threads'),
        (perm_posts_create, 'Posts_Create', 'Content', 'Create new posts'),
        (perm_posts_edit, 'Posts_Edit', 'Content', 'Edit existing posts'),
        (perm_posts_delete, 'Posts_Delete', 'Content', 'Delete posts'),
        (perm_posts_update, 'Posts_Update', 'Content', 'Update posts'),
        (perm_posts_post_image, 'Posts_PostImage', 'Content', 'Post images'),
        (perm_posts_post_reply, 'Posts_PostReply', 'Content', 'Post replies'),
        (perm_topics_create, 'Topics_Create', 'Content', 'Create new topics'),
        (perm_topics_edit, 'Topics_Edit', 'Content', 'Edit existing topics'),
        (perm_topics_delete, 'Topics_Delete', 'Content', 'Delete topics'),
        (perm_chat_create, 'Chat_Create', 'General', 'Create new chat messages'),
        (perm_chat_edit, 'Chat_Edit', 'General', 'Edit existing chat messages'),
        (perm_chat_delete, 'Chat_Delete', 'General', 'Delete chat messages'),
        (perm_chat_post_image, 'Chat_PostImage', 'General', 'Post images in chat'),
        (perm_gallery_upload_image, 'Gallery_UploadImage', 'General', 'Upload images'),
        (perm_gallery_delete_image, 'Gallery_DeleteImage', 'General', 'Delete images'),
        (perm_gallery_edit_image, 'Gallery_EditImage', 'General', 'Edit images')
        (perm_mod_moderate_posts, 'Mod_ModeratePosts', 'Moderation', 'Moderate posts'),
        (perm_mod_pin_thread, 'Mod_PinThread', 'Moderation', 'Pin threads'),
        (perm_mod_lock_thread, 'Mod_LockThread', 'Moderation', 'Lock threads'),
        (perm_mod_move_thread, 'Mod_MoveThread', 'Moderation', 'Move threads'),
        (perm_mod_ban_user, 'Mod_BanUser', 'Moderation', 'Ban users'),
        (perm_mod_mute_user, 'Mod_MuteUser', 'Moderation', 'Mute users'),
        (perm_mod_view_reported_content, 'Mod_ViewReportedContent', 'Moderation', 'View reported content'),
        (perm_mod_restore_post, 'Mod_RestorePost', 'Moderation', 'Restore posts'),
        (perm_mod_warn_user, 'Mod_WarnUser', 'Moderation', 'Warn users');
    */
}
