using MySqlConnector;
using RestApiServer.Core.Config;
using RestApiServer.Db;
using RestApiServer.Db.Users;

namespace ServerTests.testdatabase
{
    public class DatabaseFixture : ICollectionFixture<DatabaseFixture>, IDisposable
    {
        //Database server connection details:
        private static readonly string MYSQL_SERVER = "localhost";
        private static readonly string MYSQL_SERVER_PORT = "3306";
        private static readonly string MYSQL_USERNAME = "root";
        private static readonly string MYSQL_PASSWORD = "c3r3s123";
        private static readonly string MYSQL_TEST_DB_NAME = $"_community_forum_local_test_{DateTime.Now:yyyy_MM_dd_hh_mm_ss}";
        public DatabaseFixture()
        {
            SetTestEnvironmentVars();
            ConfigureTestEnvVars();
        }
        private static void ConfigureTestEnvVars()
        {
            try
            {
                Environment.SetEnvironmentVariable("ENVIRONMENT_NAME", "Testing");
                Environment.SetEnvironmentVariable("MYSQL_SERVER", MYSQL_SERVER);
                Environment.SetEnvironmentVariable("MYSQL_SERVER_PORT", MYSQL_SERVER_PORT);
                Environment.SetEnvironmentVariable("MYSQL_DB_NAME", MYSQL_TEST_DB_NAME);
                Environment.SetEnvironmentVariable("MYSQL_USERNAME", MYSQL_USERNAME);
                Environment.SetEnvironmentVariable("MYSQL_PASSWORD", MYSQL_PASSWORD);
                Environment.SetEnvironmentVariable("JWT_SHARED_SECRET", "my-32-character-ultra-secure-and-ultra-long-secret");
                ConfigurationLoader.LoadConfig();
            }
           
            catch(Exception ex)
            {
                Console.WriteLine($"Could not load environment variables: {ex.Message}. Attempting alternate method.");
                SetTestEnvironmentVars();
                ConfigurationLoader.LoadConfig();
            }
            CreateTestDatabase();
            SeedTestDatabase();
        }

        public static void SetTestEnvironmentVars()
        {
            ConfigurationLoader.SetConfigValue(EnvironmentVariable.MySqlServer, MYSQL_SERVER);
            ConfigurationLoader.SetConfigValue(EnvironmentVariable.MySqlServerPort, MYSQL_SERVER_PORT);
            ConfigurationLoader.SetConfigValue(EnvironmentVariable.MySqlDbName, MYSQL_TEST_DB_NAME);
            ConfigurationLoader.SetConfigValue(EnvironmentVariable.MySqlUsername, MYSQL_USERNAME);
            ConfigurationLoader.SetConfigValue(EnvironmentVariable.MySqlPassword, MYSQL_PASSWORD);	
            ConfigurationLoader.SetConfigValue(EnvironmentVariable.JwtSharedSecret, "my-32-character-ultra-secure-and-ultra-long-secret");
            ConfigurationLoader.SetConfigValue(EnvironmentVariable.EnvironmentName, "Testing");
        }

        private static void CreateTestDatabase()
        {
            using var db = new AppDbContext();
            db.Database.EnsureCreated();
            string connectionString = $"server={MYSQL_SERVER};port={MYSQL_SERVER_PORT};database={MYSQL_TEST_DB_NAME};user={MYSQL_USERNAME};password={MYSQL_PASSWORD}";
            using var sqlConn = new MySqlConnection(connectionString);
            sqlConn.Open();
            //Not using triggers in the forum database for now, so this can be left empty.

            sqlConn.Close();
        }

        private static void SeedTestDatabase()
        {
            CreateTestUser1();
            CreateTestAdminRole();
        }

        public static void CreateTestUser1()
        {
            using var db = new AppDbContext();
            var testUser = new UserEntry
            {
                UserId = "1",
                Username = "testuser1",
                EmailAddress = "testuser1@localhost",
                HashedPassword = BCrypt.Net.BCrypt.HashPassword("testuser1"),
                RoleId = "0",
            };

            db.Add(testUser);
            db.SaveChanges();
        }

        public static void CreateTestAdminRole()
        {
            using var db = new AppDbContext();
            var testRole = new RoleEntry
            {
                RoleId = "0",
                RoleName = "admin",
                RoleType = RestAPIServer.Enums.RoleType.Admin,
                Description = "Admin role"
            };

            db.Add(testRole);
            db.SaveChanges();
        }

        public void Dispose()
        {

            //Clean up test database
            using var db = new AppDbContext();
            db.Database.EnsureDeleted();
        }

    }
}