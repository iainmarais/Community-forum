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
            .Property(prop => prop.Permission)
            .HasConversion(GetEnumValueConverter<SystemPermissionType>());

            modelBuilder.Entity<PermissionEntry>()
            .HasKey(p => p.PermissionId);

            modelBuilder.Entity<RoleEntry>()
            .HasKey(r => r.RoleId);

            modelBuilder.Entity<RolePermissionEntry>()
            .HasKey(rp => rp.RolePermissionId);


            //Foreign key relationships for the RolePermissionEntry table
            modelBuilder.Entity<RolePermissionEntry>()
            .HasOne(rp => rp.Role)
            .WithMany()
            .HasForeignKey(rp => rp.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RolePermissionEntry>()
            .HasOne(rp => rp.Permission)
            .WithMany()
            .HasForeignKey(rp => rp.PermissionId)
            .OnDelete(DeleteBehavior.Cascade);

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

        }
        
        //Helpers
        public static ValueConverter GetEnumValueConverter<T>()
        {
            return new ValueConverter<T, string> (
                v => DbUtils.EnumNameToString(v),
                v => DbUtils.ParseEnumFromString<T>(v)
            );
        }
    }
}