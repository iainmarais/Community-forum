using RestApiServer.Core.Config;
using RestApiServer.Db.Users;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using RestApiServer.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RestApiServer.Utils;

namespace RestApiServer.Db
{
    public class AppDbContext : DbContext
    {
        //Backend
        public DbSet<UserEntry> Users { get; set; } = null!;
        public DbSet<SystemPermissionEntry> SystemPermissions { get; set; } = null!;
        public DbSet<PermissionEntry> Permissions { get; set; } = null!;
        public DbSet<RoleEntry> Roles { get; set; } = null!;
        public DbSet<UserPermissionEntry> UserPermissions { get; set; } = null!;
        public DbSet<UserRefreshTokenEntry> UserRefreshTokens { get; set; } = null!;
        public DbSet<PostEntry> Posts { get; set; } = null!;
        public DbSet<ThreadEntry> Threads { get; set; } = null!;
        public DbSet<TopicEntry> Topics { get; set; } = null!;
        public DbSet<CategoryEntry> Categories { get; set; } = null!;
        public DbSet<BoardEntry> Boards { get; set; } = null!;
        public DbSet<RolePermissionEntry> RolePermissions { get; set; } = null!;
        public DbSet<ChatMessageEntry> ChatMessages { get; set; } = null!;
        public DbSet<ChatGroupEntry> ChatGroups { get; set; } = null!;
        public DbSet<GalleryItemEntry> GalleryItems { get; set; } = null!;
        public DbSet<ContactEntry> Contacts { get; set; } = null!;
        public DbSet<ChatEntry> Chats { get; set; } = null!;


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var conn = GetConnectionString(usePooling: true, minPoolSize: 10, maxPoolSize: 50);
            var serverVersion = ServerVersion.AutoDetect(conn);
            optionsBuilder.UseMySql(conn, serverVersion, options =>
            {
                options.MigrationsAssembly("RestApiServer");
            });
        }

        public static string GetConnectionString(bool usePooling = false, int minPoolSize = 0, int maxPoolSize = 100)
        {
            var dbserver = ConfigurationLoader.GetConfigValue(EnvironmentVariable.MySqlServer);
            var dbport = ConfigurationLoader.GetConfigValue(EnvironmentVariable.MySqlServerPort);
            var dbname = ConfigurationLoader.GetConfigValue(EnvironmentVariable.MySqlDbName);
            var dbuser = ConfigurationLoader.GetConfigValue(EnvironmentVariable.MySqlUsername);
            var dbpass = ConfigurationLoader.GetConfigValue(EnvironmentVariable.MySqlPassword);
            if(usePooling)
            {
                return $"server={dbserver};port={dbport};database={dbname};user={dbuser};password={dbpass};Pooling=true;Min Pool Size={minPoolSize};Max Pool Size={maxPoolSize};";
            }
            else
            {
                return $"server={dbserver};port={dbport};database={dbname};user={dbuser};password={dbpass}";
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoleEntry>()
            .Property(prop => prop.RoleType)
            .HasConversion(GetEnumValueConverter<RoleType>());

            modelBuilder.Entity<SystemPermissionEntry>()
            .Property(prop => prop.SystemPermissionType)
            .HasConversion(GetEnumValueConverter<SystemPermissionType>());

            modelBuilder.Entity<PermissionEntry>()
            .Property(prop => prop.PermissionType)
            .HasConversion(GetEnumValueConverter<PermissionType>());

            modelBuilder.Entity<PermissionEntry>()
            .HasKey(p => p.PermissionId);

            modelBuilder.Entity<RoleEntry>()
            .HasKey(r => r.RoleId);

            modelBuilder.Entity<RolePermissionEntry>()
            .HasKey(rp => rp.RolePermissionId);

            modelBuilder.Entity<ContactEntry>()
            .HasKey(c => c.ContactId);

            modelBuilder.Entity<GalleryItemEntry>()
            .HasKey(gi => gi.GalleryItemId);

            modelBuilder.Entity<UserEntry>()
            .HasKey(u => u.UserId);

            modelBuilder.Entity<GalleryItemEntry>()
            .HasOne(g => g.CreatedByUser) // Navigation property on GalleryItem
            .WithMany(u => u.GalleryItems) // Navigation property on UserEntry
            .HasForeignKey(g => g.CreatedByUserId) // Foreign key on GalleryItem
            .OnDelete(DeleteBehavior.Restrict); // Optional: Set delete behavior

            //Foreign key relationships for the RolePermissionEntry table
            modelBuilder.Entity<RolePermissionEntry>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RolePermissionEntry>()
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RolePermissionEntry>()
                .HasIndex(rp => rp.PermissionId);

            modelBuilder.Entity<RolePermissionEntry>()
                .HasIndex(rp => rp.RoleId);

            //One to many relationships
            modelBuilder.Entity<ThreadEntry>()
            .HasMany(t => t.Posts)
            .WithOne(m => m.Thread)
            .HasForeignKey(m => m.ThreadId);

            modelBuilder.Entity<ThreadEntry>()
            .HasOne(t => t.CreatedByUser)
            .WithMany(u => u.ThreadsCreated)
            .HasForeignKey(t => t.CreatedByUserId);

            modelBuilder.Entity<TopicEntry>()
            .HasMany(t => t.Threads)
            .WithOne(tt => tt.Topic)
            .HasForeignKey(tt => tt.TopicId);

            modelBuilder.Entity<TopicEntry>()
            .HasOne(t => t.CreatedByUser)
            .WithMany(u => u.TopicsCreated)
            .HasForeignKey(t => t.CreatedByUserId);

            modelBuilder.Entity<BoardEntry>()
            .HasMany(c => c.TopicsCreated)
            .WithOne(t => t.Board)
            .HasForeignKey(t => t.BoardId);

            modelBuilder.Entity<CategoryEntry>()
            .HasMany(c => c.BoardsCreated)
            .WithOne(b => b.Category)
            .HasForeignKey(b => b.CategoryId);

            //User permissions
            modelBuilder.Entity<UserPermissionEntry>()
                .HasKey(up => up.UserPermissionId); // Primary Key

            modelBuilder.Entity<UserPermissionEntry>()
                .HasOne(up => up.User)
                .WithMany(u => u.UserPermissions)
                .HasForeignKey(up => up.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserPermissionEntry>()
                .HasOne(up => up.SystemPermission)
                .WithMany(sp => sp.UserPermissions)
                .HasForeignKey(up => up.SystemPermissionId)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<UserRefreshTokenEntry>()
                .HasOne(urt => urt.User)
                .WithMany(u => u.UserRefreshTokens)
                .HasForeignKey(urt => urt.AssignedToUserId);

            // Indexes
            modelBuilder.Entity<UserPermissionEntry>()
                .HasIndex(up => up.UserId);

            modelBuilder.Entity<UserPermissionEntry>()
                .HasIndex(up => up.SystemPermissionId);            

            modelBuilder.Entity<UserRefreshTokenEntry>()
                .HasKey(urt => urt.UserRefreshTokenId);

            //Seed data - important structures only.
            modelBuilder.Entity<RoleEntry>().HasData(
                //Use the preset roles to seed this.
                PresetRoles.ToArray()
            );

            modelBuilder.Entity<SystemPermissionEntry>().HasData(
                PresetSystemPermissions.ToArray()
            );

            modelBuilder.Entity<PermissionEntry>().HasData(
                PresetPermissions.ToArray()
            );
        }
        
        //Helpers
        public static ValueConverter GetEnumValueConverter<T>()
        {
            return new ValueConverter<T, string> (
                v => DbUtils.EnumNameToString(v),
                v => DbUtils.ParseEnumFromString<T>(v)
            );
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
                SystemPermissionId = "interactive_upload_images",
                SystemPermissionName = "Interactive: Upload images",
                SystemPermissionType = SystemPermissionType.Interactivity,
                Description = "Allows a user to upload images to the gallery."
            },
            new SystemPermissionEntry
            {
                SystemPermissionId = "interactive_edit_posts",
                SystemPermissionName = "Interactive: Edit posts",
                SystemPermissionType = SystemPermissionType.Interactivity,
                Description = "Allows a user to edit their own posts."
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
                CategoryId = "General",
                CategoryName = "General",
                CategoryDescription = "General discussions for the forum.",
            },
            new CategoryEntry
            {
                CategoryId = "ITSupport",
                CategoryName = "Computer and IT Support",
                CategoryDescription = "Everything pertaining to computer and IT support can be discussed here.",
            },
            new CategoryEntry
            {
                CategoryId = "Development",
                CategoryName = "Software development",
                CategoryDescription = "Everything pertaining to software development can be discussed here.",
            }
        };
    }
}


