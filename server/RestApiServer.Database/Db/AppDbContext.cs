using RestApiServer.Common.Config;
using RestApiServer.Db.Users;
using Microsoft.EntityFrameworkCore;
using RestApiServer.CommonEnums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RestApiServer.Db.Ops;
using RestApiServer.Database.Utils;
using Serilog;
using MySqlConnector;
using RestApiServer.Database.Db;

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
        public DbSet<UserSessionTokenEntry> UserSessionTokens { get; set; } = null!;
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
        public DbSet<BannedUserEntry> BannedUsers { get; set; } = null!;
        public DbSet<RequestEntry> Requests { get; set; } = null!;

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var conn = GetConnectionString(usePooling: true, minPoolSize: 10, maxPoolSize: 50);
            try
            {  
                //Get the server version. If we can't get it it might help to raise an exception here that can be passed through to the app builder and handled there.
                ServerVersion? serverVersion = ServerVersion.AutoDetect(conn);
                optionsBuilder.UseMySql(conn, serverVersion, options =>
                {
                    options.MigrationsAssembly("RestApiServer.Database");
                });
            }
            catch (MySqlException ex)
            {
                Log.Fatal($"Could not connect to a database: {ex.Message}", ex);
                NotifyUserAndShutDown();
                //Todo: Use SQLite as a fallback? Or do we simply terminate all further execution?
            }
            //See where keys are conflicting
            optionsBuilder.EnableSensitiveDataLogging();
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

            modelBuilder.Entity<BannedUserEntry>()
            .Property(prop => prop.BanType)
            .HasConversion(GetEnumValueConverter<BanType>());

            modelBuilder.Entity<RequestEntry>()
            .Property(prop => prop.TriageStatus)
            .HasConversion(GetEnumValueConverter<TriageStatus>());

            modelBuilder.Entity<RequestEntry>()
            .Property(prop => prop.TriageType)
            .HasConversion(GetEnumValueConverter<TriageType>());

            modelBuilder.Entity<PermissionEntry>()
            .HasKey(p => p.PermissionId);

            modelBuilder.Entity<RequestEntry>()
            .HasKey(sr => sr.RequestId);

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

            //Support request entry
            modelBuilder.Entity<RequestEntry>()
                .HasIndex(sr => sr.RequestId);

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

            //Support request foreign key relations
            // One-to-many relationship for CreatedByUserId (User can create many requests)
            modelBuilder.Entity<RequestEntry>()
                .HasOne(s => s.CreatedByUser)
                .WithMany(u => u.CreatedSupportRequests) // Navigation property in UserEntry for created requests
                .HasForeignKey(s => s.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
                
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
            
            modelBuilder.Entity<UserRefreshTokenEntry>()
                .HasIndex(urt => urt.RefreshToken)
                .IsUnique();

            modelBuilder.Entity<UserSessionTokenEntry>()
                .HasIndex(ust => ust.SessionToken)
                .IsUnique();
            
            modelBuilder.Entity<UserSessionTokenEntry>()
                .HasKey(ust => ust.UserSessionTokenId);

            modelBuilder.Entity<UserSessionTokenEntry>()
                .HasOne(ust => ust.User)
                .WithMany(u => u.UserSessionTokens)
                .HasForeignKey(ust => ust.AssignedToUserId);

            DbOps.SeedData(modelBuilder);
        }

        //Helpers
        public static ValueConverter GetEnumValueConverter<T>()
        {
            return new ValueConverter<T, string> (
                v => DbUtils.EnumNameToString(v),
                v => DbUtils.ParseEnumFromString<T>(v)
            );
        }  
        //Notify the user the server is unable to start due to a missing database engine
        private static void NotifyUserAndShutDown()
        {
            Log.Information("This server requires a database, such as MySQL or MariaDB in order to start.\nPlease refer to the README.md for more information.\n");
            Log.Information("MariaDB can be downloaded from https://mariadb.com/downloads/ \nMySQL can be downloaded from https://dev.mysql.com/downloads/");
            Log.Information("Server will now shut down. Press any key to exit...");
            Console.ReadKey();
            Environment.Exit(1);
        }
    }
}


