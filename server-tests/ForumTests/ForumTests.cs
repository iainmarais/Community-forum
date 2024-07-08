
using RestApiServer.Db;
using RestApiServer.Db.Users;

namespace ServerTests.ForumTests
{

    [Collection("Database Collection")]
    public class ForumTests
    {

        //User service tests

        [Fact]
        public void CreateUserTest()
        {
            var testUser = new UserEntry
            {
                UserId = "2",
                Username = "testuser2",
                EmailAddress = "testuser2@localhost",
                HashedPassword = BCrypt.Net.BCrypt.HashPassword("testuser2"),
                RoleId = "0",
            };

            using var db = new AppDbContext();
            db.Add(testUser);
            db.SaveChanges();

            using var TestDb = new AppDbContext();

            Assert.Equal("testuser2", TestDb.Users.Single(u => u.UserId == "2").Username);
            Assert.Equal("testuser2@localhost", TestDb.Users.Single(u => u.UserId == "2").EmailAddress);
            Assert.Equal("2", TestDb.Users.Single(u => u.UserId == "2").UserId);
            Assert.Equal("0", TestDb.Users.Single(u => u.UserId == "2").RoleId);
        }

        //Forum related tests
    }
}