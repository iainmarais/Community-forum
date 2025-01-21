
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
                UserId = "3",
                Username = "testuser3",
                EmailAddress = "testuser3@localhost",
                HashedPassword = BCrypt.Net.BCrypt.HashPassword("testuser3"),
                RoleId = "0",
            };

            using var db = new AppDbContext();
            db.Users.Add(testUser);
            db.SaveChanges();

            using var TestDb = new AppDbContext();

            Assert.Equal("testuser3", TestDb.Users.Single(u => u.UserId == "3").Username);
            Assert.Equal("testuser3@localhost", TestDb.Users.Single(u => u.UserId == "3").EmailAddress);
            Assert.Equal("3", TestDb.Users.Single(u => u.UserId == "3").UserId);
            Assert.Equal("0", TestDb.Users.Single(u => u.UserId == "3").RoleId);
        }

        //Forum-related tests
        //Category tests
        [Fact]
        public void CreateCategoryTest()
        {
            var testCategory = new CategoryEntry
            {
                CategoryId = "1",
                CategoryName = "Test Category",
                CategoryDescription = "This is a test category",
                CreatedByUserId = "3",
            };

            using var db = new AppDbContext();
            db.Categories.Add(testCategory);
            db.SaveChanges();

            using var checkDb = new AppDbContext();
            var checkCategory = checkDb.Categories.Single(c => c.CategoryId == "1");
            
            Assert.Equal("1", checkCategory.CategoryId);
            Assert.Equal("Test Category", checkCategory.CategoryName);
            Assert.Equal("This is a test category", checkCategory.CategoryDescription);
            Assert.Equal("3", checkCategory.CreatedByUserId);
        }
        //Board tests
        [Fact]
        public void CreateBoardTest()
        {
            var testCategory = new CategoryEntry
            {
                CategoryId = "2",
                CategoryName = "Test Category",
                CategoryDescription = "This is a test category",
                CreatedByUserId = "3",
            };

            var testBoard = new BoardEntry
            {
                BoardId = "1",
                BoardName = "Test Board",
                BoardDescription = "This is a test board",
                CreatedByUserId = "3",
                CategoryId = "2",
            };

            using var db = new AppDbContext();

            db.Boards.Add(testBoard);
            db.Categories.Add(testCategory);
            db.SaveChanges();

            using var checkDb = new AppDbContext();
            var checkBoard = checkDb.Boards.Single(b => b.BoardId == "1");

            Assert.Equal("1", checkBoard.BoardId);
            Assert.Equal("Test Board", checkBoard.BoardName);
            Assert.Equal("This is a test board", checkBoard.BoardDescription);
            Assert.Equal("3", checkBoard.CreatedByUserId);
            Assert.Equal("2", checkBoard.CategoryId);
        }
    }
}