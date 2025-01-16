using Microsoft.EntityFrameworkCore;
using RestApiServer.CommonEnums;
using Serilog;

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
            modelBuilder.Entity<RolePermissionEntry>().HasData(PresetRolePermissions.ToArray());
            modelBuilder.Entity<CategoryEntry>().HasData(PresetCategories.ToArray());
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
                RoleId = "CommunityManager",
                RoleType = RoleType.CommunityManager,
                RoleName = "Community Manager",
                Description = "Community managers are trusted community members of the forum."
            },
            new RoleEntry
            {
                RoleId = "ContentCreator",
                RoleType = RoleType.ContentCreator,
                RoleName = "Content creator",
                Description = "Community members who create high-quality content."
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

        public static List<RolePermissionEntry> PresetRolePermissions = new()
        {
            //Create our role to permission mappings here. Once done, this will enable a more granular approach to handling user rights assignment.
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

            //Content permissions
            new SystemPermissionEntry
            {
                SystemPermissionId = "cnt_post_images",
                SystemPermissionName = "Content: Post images",
                SystemPermissionType = SystemPermissionType.Content,
                Description = "Allows users to post images",
            },
            new SystemPermissionEntry
            {
                SystemPermissionId = "cnt_post_audio_clips",
                SystemPermissionName = "Content: Post audio clips",
                SystemPermissionType = SystemPermissionType.Content,
                Description = "Allows users to post audio clips",
            },
            new SystemPermissionEntry
            {
                SystemPermissionId = "cnt_post_videos",
                SystemPermissionName = "Content: Post videos",
                SystemPermissionType = SystemPermissionType.Content,
                Description = "Allows users to post videos",
            },
            new SystemPermissionEntry
            {
                SystemPermissionId = "cnt_delete_items",
                SystemPermissionName = "Content: Delete items",
                SystemPermissionType = SystemPermissionType.Content,
                Description = "Allows users to delete content",
            },
            //Content permissions relating to posts, threads and boards. Include: Permissions to create, update and view boards, topics and threads
            new SystemPermissionEntry
            {
                SystemPermissionId = "cnt_create_boards",
                SystemPermissionName = "Content: Create boards",
                SystemPermissionType = SystemPermissionType.Content,
                Description = "Allows users to create new boards",
            },
            new SystemPermissionEntry
            {
                SystemPermissionId = "cnt_create_topics",
                SystemPermissionName = "Content: Create topics",
                SystemPermissionType = SystemPermissionType.Content,
                Description = "Allows users to create new topics",
            },
            new SystemPermissionEntry
            {
                SystemPermissionId = "cnt_create_threads",
                SystemPermissionName = "Content: Create threads",
                SystemPermissionType = SystemPermissionType.Content,
                Description = "Allows users to create new threads",
            },
            new SystemPermissionEntry
            {
                SystemPermissionId = "cnt_update_posts",
                SystemPermissionName = "Content: Update posts",
                SystemPermissionType = SystemPermissionType.Content,
                Description = "Allows users to update their own posts",
            },
            new SystemPermissionEntry
            {
                SystemPermissionId = "cnt_update_threads",
                SystemPermissionName = "Content: Update threads",
                SystemPermissionType = SystemPermissionType.Content,
                Description = "Allows users to update their own threads",
            },
            new SystemPermissionEntry
            {
                SystemPermissionId = "cnt_update_topics",
                SystemPermissionName = "Content: Update topics",
                SystemPermissionType = SystemPermissionType.Content,
                Description = "Allows users to update their own topics",
            },
            new SystemPermissionEntry
            {
                SystemPermissionId = "cnt_update_boards",
                SystemPermissionName = "Content: Update boards",
                SystemPermissionType = SystemPermissionType.Content,
                Description = "Allows users to update their own boards",
            },
            //Content permissions for deleting topics and threads. 
            //Idea for functionality update: Allow soft-delete via "IsMarkedForDelete=true" and "DateMarkedForDelete=DateTime.Now" where DateTime.Now refers to the date the IsMarkedForDelete prop is set true...
            //Benefit of this code/database update: Boards, threads, topics, posts, categories etc can be marked for delete, which can then hide them from the frontend and also enable them to be purged after a certain period of time.
            //Also, by using IsMarkedForDelete = true and another bool such as IsImportant = true, the elements in question can be virtually deleted while remaining, and only an administrator logged in to the admin portal can view deleted content. 
            //Important content can't be deleted even though it is virtually deleted. As an additional safeguard, only approved users should be able to view such content.
            new SystemPermissionEntry
            {
                SystemPermissionId = "cnt_delete_topics",
                SystemPermissionName = "Content: Delete topics",
                SystemPermissionType = SystemPermissionType.Content,
                Description = "Allows users to delete topics",
            },
            new SystemPermissionEntry
            {
                SystemPermissionId = "cnt_delete_threads",
                SystemPermissionName = "Content: Delete threads",
                SystemPermissionType = SystemPermissionType.Content,
                Description = "Allows users to delete threads",
            },
            new SystemPermissionEntry
            {
                SystemPermissionId = "cnt_delete_posts",
                SystemPermissionName = "Content: Delete posts",
                SystemPermissionType = SystemPermissionType.Content,
                Description = "Allows users to delete posts",
            },
            new SystemPermissionEntry
            {
                SystemPermissionId = "cnt_delete_boards",
                SystemPermissionName = "Content: Delete boards",
                SystemPermissionType = SystemPermissionType.Content,
                Description = "Allows users to delete boards",
            },            
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
                CreatedByUserId = ""
            },
            new CategoryEntry
            {
                CategoryId = Guid.NewGuid().ToString(),
                CategoryName = "Computer and IT Support",
                CategoryDescription = "Everything pertaining to computer and IT support can be discussed here.",
                CreatedByUserId = ""
            },
            new CategoryEntry
            {
                CategoryId = Guid.NewGuid().ToString(),
                CategoryName = "Software development",
                CategoryDescription = "Everything pertaining to software development can be discussed here.",
                CreatedByUserId = ""
            }
        };

        public static async Task SeedDataAsync(bool forceSeed = false, bool updateData = false)
        {
            //Create our db context instance.
            using var db  = new AppDbContext();
            try
            {
                if(!db.Roles.Any())
                {
                    await db.Roles.AddRangeAsync(PresetRoles);
                    await db.SaveChangesAsync();
                }
                if(!db.SystemPermissions.Any())
                {
                    await db.SystemPermissions.AddRangeAsync(PresetSystemPermissions);
                    await db.SaveChangesAsync();
                }
                if(!db.Permissions.Any())
                {
                    await db.Permissions.AddRangeAsync(PresetPermissions);
                    await db.SaveChangesAsync();
                }
                if(!db.RolePermissions.Any())
                {
                    await db.RolePermissions.AddRangeAsync(PresetRolePermissions);
                    await db.SaveChangesAsync();
                }
                if(forceSeed == true)
                {
                    await db.Roles.AddRangeAsync(PresetRoles);
                    await db.SystemPermissions.AddRangeAsync(PresetSystemPermissions);
                    await db.Permissions.AddRangeAsync(PresetPermissions);
                    await db.RolePermissions.AddRangeAsync(PresetRolePermissions);
                    await db.SaveChangesAsync();
                }
                //This branch can do with a rewrite to make it more concise... 
                if(updateData == true)
                {
                    foreach (var role in PresetRoles)
                    {
                        var existingRole = await db.Roles.FindAsync(role.RoleId);
                        if (existingRole == null)
                        {
                            await db.Roles.AddAsync(role);
                        }
                        else
                        {
                            //Update the existing role data.
                            existingRole.RoleName = role.RoleName;
                            existingRole.RoleType = role.RoleType;
                            existingRole.Description = role.Description;
                        }
                    }
                    foreach (var permission in PresetPermissions)
                    {
                        var existingPermission = await db.Permissions.FindAsync(permission.PermissionId);
                        if(existingPermission == null)
                        {
                            await db.Permissions.AddAsync(permission);
                        }
                        else
                        {
                            //Update the existing entry, bar the PermissionId.
                            existingPermission.PermissionName = permission.PermissionName;
                            existingPermission.PermissionType = permission.PermissionType;
                        }
                    }
                    foreach (var systemPermission in PresetSystemPermissions)
                    {
                        var existingSystemPermission = await db.SystemPermissions.FindAsync(systemPermission.SystemPermissionId);
                        if(existingSystemPermission == null)
                        {
                            await db.SystemPermissions.AddAsync(systemPermission);
                        }
                        else
                        {
                            //Update it.
                            existingSystemPermission.SystemPermissionName = systemPermission.SystemPermissionName;
                            existingSystemPermission.SystemPermissionType = systemPermission.SystemPermissionType;
                        }
                    }
                    foreach (var rolePermission in PresetRolePermissions)
                    {
                        var existingRolePermission = await db.RolePermissions.FindAsync(rolePermission.RolePermissionId);
                        if(existingRolePermission == null)
                        {
                            await db.RolePermissions.AddAsync(rolePermission);
                        }
                        else
                        {
                            existingRolePermission.RoleId = rolePermission.RoleId;
                            existingRolePermission.PermissionId = rolePermission.PermissionId;
                        }
                    }
                    //Save the changes.
                    await db.SaveChangesAsync();
                }
                else
                {
                    //Nothing to do.
                    return;   
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Something went wrong during seeding the database: {ex.Message} \n" +
                 $"Stack trace: {ex.StackTrace}");
            }
        }
    }
}