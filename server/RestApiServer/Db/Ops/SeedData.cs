using Microsoft.EntityFrameworkCore;
using RestApiServer.Enums;

namespace RestApiServer.Db.Ops
{
    public static class DbOps
    {
        public static void SeedData(ModelBuilder modelBuilder)
        {
            //Seed the data.
            modelBuilder.Entity<RoleEntry>().HasData(PresetRoles.ToArray());
            modelBuilder.Entity<PermissionEntry>().HasData(PresetPermissions.ToArray());
            modelBuilder.Entity<SystemPermissionEntry>().HasData(PresetSystemPermissions.ToArray());
        }        
        
        public static List<RoleEntry> PresetRoles = new()
        {
            new RoleEntry 
            { 
                RoleId="Admin", 
                RoleType=RoleType.Admin, 
                RoleName="Administrator", 
                Description="Administrators have unrestricted access to administrate the forum and chat services.", 
            },
            new RoleEntry 
            { 
                RoleId="Moderator", 
                RoleType=RoleType.Moderator, 
                RoleName="Moderator", 
                Description="Moderators are trusted community members of the forum.",                
            },
            new RoleEntry 
            { 
                RoleId="JuniorModerator", 
                RoleType=RoleType.JuniorModerator, 
                RoleName="Junior Moderator", 
                Description="Moderators are trusted community members of the forum.",               
            },
            new RoleEntry 
            { 
                RoleId="User", 
                RoleType=RoleType.User, 
                RoleName="Regular User", 
                Description="Users have limited rights to the forum, but can create posts and upload content, and edit their own posts.",                
            },
            new RoleEntry
            {
                RoleId = "Guest",
                RoleType = RoleType.Guest,
                RoleName="Guest",
                Description = "Guests have limited rights to the forum, and can only post in authorised areas."
            }
        };
        public static List<PermissionEntry> PresetPermissions = new()
        { 
            //Basic user permissions - these are tied to roles:
            new PermissionEntry
            {
                PermissionId = "content_createPosts",
                PermissionName = "Content: Create Posts",
                PermissionType = PermissionType.Content,
                Description = "Allows users and guests to create new posts, though with some restrictions on guests.",
            },
            new PermissionEntry
            {
                PermissionId = "content_createThreads",
                PermissionName = "Content: Create Threads",
                PermissionType = PermissionType.Content,
                Description = "Allows registered users to create new threads.",
            },
            new PermissionEntry
            {
                PermissionId = "content_uploadImages",
                PermissionName = "Content: Upload images",
                PermissionType = PermissionType.Content,
                Description = "Allows a user to upload images to the gallery."
            },
            
        };

        public static List<SystemPermissionEntry> PresetSystemPermissions = new()
        {
            //Development permissions
            new SystemPermissionEntry
            {
                SystemPermissionId = "dev_view_areas_under_construction",
                SystemPermissionName = "Development: View areas under construction",
                SystemPermissionType = SystemPermissionType.Development,
                Description = "Allows a user to view areas under construction. This is for development purposes only.",
            },
            //Testing permissions

            //Production permissions

            //Visibility permissions
            new SystemPermissionEntry
            {
                SystemPermissionId = "vis_view_hidden_content",
                SystemPermissionName = "Visibility: View hidden content",
                SystemPermissionType = SystemPermissionType.Visibility,
                Description = "Allows a user to view hidden content.",
            },
            new SystemPermissionEntry
            {
                SystemPermissionId = "vis_view_deleted_posts",
                SystemPermissionName = "Visibility: View deleted posts",
                SystemPermissionType = SystemPermissionType.Visibility,
                Description = "Allows a user to view deleted posts.",
            },
            new SystemPermissionEntry
            {
                SystemPermissionId = "vis_view_banned_users",
                SystemPermissionName = "Visibility: View banned users",
                SystemPermissionType = SystemPermissionType.Visibility,
                Description = "Allows a user to view banned users.",  
            },
            new SystemPermissionEntry
            {
                SystemPermissionId = "vis_view_user_activity",
                SystemPermissionName = "Visibility: View user activity",
                SystemPermissionType = SystemPermissionType.Visibility,
                Description = "Allows a user to view user activity.",
            },
            //Interactivity permissions
            new SystemPermissionEntry
            {
                SystemPermissionId = "interactive_create_posts",
                SystemPermissionName = "Interactive: Create posts",
                SystemPermissionType = SystemPermissionType.Interactivity,
                Description = "Allows a user to create new posts. All registered users have such permission, aside from guests, who may only post in authorised areas.",
            },
            new SystemPermissionEntry
            {
                SystemPermissionId = "interactive_create_boards",
                SystemPermissionName = "Interactive: Create boards",
                SystemPermissionType = SystemPermissionType.Interactivity,
                Description = "Allows a user to create new boards. All registered users have such permission, aside from guests, who may only post in authorised areas.",
            },
            new SystemPermissionEntry
            {
                SystemPermissionId = "interactive_create_threads",
                SystemPermissionName = "Interactive: Create threads",
                SystemPermissionType = SystemPermissionType.Interactivity,
                Description = "Allows a user to create new threads. All registered users have such permission, aside from guests, who may only post in authorised areas.",
            },
            //Interactivity: Editing
            new SystemPermissionEntry
            {
                SystemPermissionId = "interactive_edit_posts",
                SystemPermissionName = "Interactive: Edit posts",
                SystemPermissionType = SystemPermissionType.Interactivity,
                Description = "Allows a user to edit existing posts. All registered users have such permission, aside from guests, who may only post in authorised areas.",
            },
            new SystemPermissionEntry
            {
                SystemPermissionId = "interactive_edit_boards",
                SystemPermissionName = "Interactive: Edit boards",
                SystemPermissionType = SystemPermissionType.Interactivity,
                Description = "Allows a user to edit existing boards. All registered users have such permission, aside from guests, who may only post in authorised areas.",
            },
            new SystemPermissionEntry
            {
                SystemPermissionId = "interactive_edit_threads",
                SystemPermissionName = "Interactive: Edit threads",
                SystemPermissionType = SystemPermissionType.Interactivity,
                Description = "Allows a user to edit existing threads. All registered users have such permission, aside from guests, who may only post in authorised areas.",
            },
            //Interactivity: Delete existing boards, threads, posts
            new SystemPermissionEntry
            {
                SystemPermissionId = "interactive_delete_posts",
                SystemPermissionName = "Interactive: Delete posts",
                SystemPermissionType = SystemPermissionType.Interactivity,
                Description = "Allows a user to delete existing posts. All registered users have such permission, aside from guests, who may only post in authorised areas.",
            },
            new SystemPermissionEntry
            {
                SystemPermissionId = "interactive_delete_boards",
                SystemPermissionName = "Interactive: Delete boards",
                SystemPermissionType = SystemPermissionType.Interactivity,
                Description = "Allows a user to delete existing boards. All registered users have such permission, aside from guests, who may only post in authorised areas.",
            },
            new SystemPermissionEntry
            {
                SystemPermissionId = "interactive_delete_threads",
                SystemPermissionName = "Interactive: Delete threads",
                SystemPermissionType = SystemPermissionType.Interactivity,
                Description = "Allows a user to delete existing threads. All registered users have such permission, aside from guests, who may only post in authorised areas.",
            },
            //General interactivity
            new SystemPermissionEntry
            {
                SystemPermissionId = "interactive_vote_in_polls",
                SystemPermissionName = "Interactive: Vote in polls",
                SystemPermissionType = SystemPermissionType.Interactivity,
                Description = "Allows a user to vote in polls.",
            },
            new SystemPermissionEntry
            {
                SystemPermissionId = "interactive_upload_images",
                SystemPermissionName = "Interactive: Upload images",
                SystemPermissionType = SystemPermissionType.Interactivity,
                Description = "Allows a user to upload images to the gallery."
            },
            new SystemPermissionEntry
            {
                SystemPermissionId = "interactive_reply_to_posts",
                SystemPermissionName = "Interactive: Reply to posts",
                SystemPermissionType = SystemPermissionType.Interactivity,
                Description = "Allows a user to reply to other users posts."
            },
            new SystemPermissionEntry
            {
                SystemPermissionId = "interactive_edit_own_posts",
                SystemPermissionName = "Interactive: Edit own posts",
                SystemPermissionType = SystemPermissionType.Interactivity,
                Description = "Allows a user to edit their own posts."
            },
            new SystemPermissionEntry
            {
                SystemPermissionId = "interactive_delete_own_posts",
                SystemPermissionName = "Interactive: Delete own posts",
                SystemPermissionType = SystemPermissionType.Interactivity,
                Description = "Allows a user to delete their own posts."
            }
            //Access permissions
            
            //General permissions
        };
        public static List<CategoryEntry> PresetCategories = new()
        {
            new CategoryEntry 
            {
                CategoryId = Guid.NewGuid().ToString(),
                CategoryName = "General",
                CategoryDescription = "General discussions for the forum.",
            },
            new CategoryEntry
            {
                CategoryId = Guid.NewGuid().ToString(),
                CategoryName = "Computer and IT Support",
                CategoryDescription = "Everything pertaining to computer and IT support can be discussed here.",
            },
            new CategoryEntry
            {
                CategoryId = Guid.NewGuid().ToString(),
                CategoryName = "Software development",
                CategoryDescription = "Everything pertaining to software development can be discussed here.",
            }
        };         
    }
}