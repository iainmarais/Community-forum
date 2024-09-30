using Microsoft.EntityFrameworkCore;
using RestApiServer.Db;
using RestApiServer.Db.Users;
using RestApiServer.Enums;

namespace RestApiServer.Utils
{
    public class DbUtils
    {
        public static string GenerateUuid()
        {
            return Guid.NewGuid().ToString();
        }

        //Context-related utility methods
        //Convert enum value to string
        public static string EnumNameToString<T>(T value)
        {
            if(value == null)
            {
                throw new Exception("Value can't be null");
            }
            return Enum.GetName(typeof(T), value) ?? throw new Exception($"Conversion failed for enum value: {value}");
        }

        public static string? EnumNameToStringNullable<T>(T? value)
        {
            if(value == null)
            {
                return null;
            }
            return Enum.GetName(typeof(T), value);
        }

        //Parse enum value from string
        public static T ParseEnumFromString<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value) ?? throw new Exception($"Enum value can't be null.");
        }

        public static T? ParseEnumFromStringNullable<T>(string? value)
        {
            if(value == null)
            {
                return default;
            }
            return (T)Enum.Parse(typeof(T), value);
        }

        public static async void SetupDatabase(bool seedData = false)
        {
            try
            {
                //Should update the database. This will create it if it does not exist yet.
                
                if (seedData)
                {
                    SeedData();
                }
                await DbContextUtils.CreateInstance().SaveChangesAsync();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                //Since during setup this is a critical fault, the program should shut down immediately.
                throw;
            }
        }

        public static void InsertData<T>(AppDbContext db, DbSet<T>? table, IEnumerable<T>? data) where T : class
        {
            try
            {
                if(data == null)
                {
                    Console.WriteLine("No data to insert");
                }
                if(table == null)
                {
                    Console.WriteLine("No table to insert into");
                }
                if(data!.Count() == 1 && table != null)
                {
                    table!.Add(data!.First());
                }
                if(data!.Count() > 1 && table != null)
                {
                    table!.AddRange(data!);
                }  
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static async void SeedData()
        {
            if (!DbContextUtils.CreateInstance().Roles.Any())
            {
                using var db = DbContextUtils.CreateInstance();
                List<RoleEntry> roleEntries = new()
                {
                    new RoleEntry
                    {
                        RoleId = DbUtils.GenerateUuid(),
                        RoleType = RoleType.Admin,
                        RoleName = "Administrator"
                    },
                    new RoleEntry 
                    {
                        RoleId = DbUtils.GenerateUuid(),
                        RoleType = RoleType.Moderator,
                        RoleName = "Moderator"
                    },
                    new RoleEntry
                    {
                        RoleId = DbUtils.GenerateUuid(),
                        RoleType = RoleType.User,
                        RoleName = "User"
                    },
                    new RoleEntry
                    {
                        RoleId = DbUtils.GenerateUuid(),
                        RoleType = RoleType.Guest,
                        RoleName = "Guest"
                    }
                };
                InsertData(db, DbContextUtils.CreateInstance().Roles, roleEntries);
            }

            if(!DbContextUtils.CreateInstance().Users.Any())
            {
                using var db = DbContextUtils.CreateInstance();
                var adminRoleId = DbContextUtils.CreateInstance().Roles.First(r => r.RoleType == RoleType.Admin).RoleId;
                List<UserEntry> userEntries = new()
                {
                    UserEntry.CreateDefaultGuestUser(),
                    new UserEntry
                    {
                        UserId = DbUtils.GenerateUuid(),
                        Username = "Admin",
                        EmailAddress = "Admin",
                        HashedPassword = BCrypt.Net.BCrypt.HashPassword("Admin"),
                        RoleId = adminRoleId
                    }
                };
                InsertData(db, DbContextUtils.CreateInstance().Users, userEntries);
            }
            //Todo: Set up role and user permissions from the system permissions, which also need to be added here.
            //Idea for system permissions: use a foreach loop? 
            //For each element found => 
                //Copy the system permission id to the system permission id on the user permission entry created.
                //Copy the system permission id to the system permission id on each role permission entry created.

            if(!DbContextUtils.CreateInstance().SystemPermissions.Any()) 
            {
                using var db = DbContextUtils.CreateInstance();
                List<string> Permissions= new() 
                {
                    "Users_Create",
                    "Users_Edit",
                    "Users_Delete",
                    "Users_ChangeRoles",
                    "Users_ChangePassword",
                    "Users_BanUser",
                    "Roles_Create",
                    "Roles_Edit",
                    "Roles_Delete",
                    "Threads_Create",
                    "Threads_Edit",
                    "Threads_Delete",
                    "Threads_Lock",
                    "Threads_Unlock",
                    "Messages_Create",
                    "Messages_Edit",
                    "Messages_Delete",
                    "Messages_Update",
                    "Messages_PostImage",
                    "Messages_PostReply",
                    "Topics_Create",
                    "Topics_Edit",
                    "Topics_Delete"
                };
                List<SystemPermissionEntry> systemPermissionEntries = new();
                foreach(var permission in Permissions)
                {
                    systemPermissionEntries.Add(new SystemPermissionEntry
                    {
                        SystemPermissionId = DbUtils.GenerateUuid(),
                        Permission = DbUtils.ParseEnumFromString<SystemPermissionType>(permission),
                        PermissionName = permission
                    });
                }
                InsertData(db, DbContextUtils.CreateInstance().SystemPermissions, systemPermissionEntries);
            }
            if(!DbContextUtils.CreateInstance().UserPermissions.Any())
            {
                using var db = DbContextUtils.CreateInstance();
            }
            if(!DbContextUtils.CreateInstance().RolePermissions.Any())
            {
                using var db = DbContextUtils.CreateInstance();
                List<RolePermissionEntry> rolePermissionEntries = new();
                var adminRole = DbContextUtils.CreateInstance().Roles.First(r => r.RoleType == RoleType.Admin);
                var moderatorRole = DbContextUtils.CreateInstance().Roles.First(r => r.RoleType == RoleType.Moderator);
                var userRole = DbContextUtils.CreateInstance().Roles.First(r => r.RoleType == RoleType.User);
                var guestRole = DbContextUtils.CreateInstance().Roles.First(r => r.RoleType == RoleType.Guest);
                foreach(var permission in DbContextUtils.CreateInstance().SystemPermissions)
                {
                    if(adminRole != null && permission.IsAdminPermission == true)
                    {
                        //Copy all permissions
                        rolePermissionEntries.Add(new RolePermissionEntry
                        {
                            RoleId = DbContextUtils.CreateInstance().Roles.First(r => r.RoleType == RoleType.Admin).RoleId,
                            RolePermissionId = DbUtils.GenerateUuid(),
                            SystemPermissionId = permission.SystemPermissionId
                        });
                    }
                    if(moderatorRole != null && permission.IsModeratorPermission == true)
                    {
                        //Copy all permissions
                        rolePermissionEntries.Add(new RolePermissionEntry
                        {
                            RoleId = DbContextUtils.CreateInstance().Roles.First(r => r.RoleType == RoleType.Moderator).RoleId,
                            RolePermissionId = DbUtils.GenerateUuid(),
                            SystemPermissionId = permission.SystemPermissionId
                        });
                    }
                    if(userRole != null && permission.IsUserPermission == true)
                    {
                        //Copy all permissions
                        rolePermissionEntries.Add(new RolePermissionEntry
                        {
                            RoleId = DbContextUtils.CreateInstance().Roles.First(r => r.RoleType == RoleType.User).RoleId,
                            RolePermissionId = DbUtils.GenerateUuid(),
                            SystemPermissionId = permission.SystemPermissionId
                        });
                    }
                    if(guestRole != null && permission.IsGuestPermission == true)
                    {
                        //Copy all permissions
                        rolePermissionEntries.Add(new RolePermissionEntry
                        {
                            RoleId = DbContextUtils.CreateInstance().Roles.First(r => r.RoleType == RoleType.Guest).RoleId,
                            RolePermissionId = DbUtils.GenerateUuid(),
                            SystemPermissionId = permission.SystemPermissionId
                        });
                    }
                }
                InsertData(db, db.RolePermissions, rolePermissionEntries);
            }
        }                
    }
}