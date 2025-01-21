using Microsoft.EntityFrameworkCore;
using RestApiServer.Db;
using RestApiServer.Db.Users;

namespace ServerTests.AdminPortalTests
{
    [Collection("Database Collection")]
    public class AdminPortalTests
    {
        [Fact]
        public void TestCreateServiceRequest()
        {
            using var db = new AppDbContext();

            var testAdminUser2 = new UserEntry
            {
                UserId = "3",
                Username = "testuser3",
                EmailAddress = "testuser3@localhost",
                HashedPassword = BCrypt.Net.BCrypt.HashPassword("testuser3"),
                RoleId = "0",
            };

            var testServiceRequest = new RequestEntry
            {
                RequestId = "002",
                CreatedByUserId = testAdminUser2.UserId,
                SupportRequestTitle = "Test Service Request",
                SupportRequestContent = "This is a test service request",
                CreatedDate = DateTime.Now,
                IsResolved = false,
                TriageStatus = RestApiServer.CommonEnums.TriageStatus.Untriaged,
                TriageType = RestApiServer.CommonEnums.TriageType.Unspecified,
            };
            db.Users.Add(testAdminUser2);
            db.Requests.Add(testServiceRequest);
            db.SaveChanges();

            using var checkDb = new AppDbContext();

            //Retrieve the user and request from the database
            var checkUser = checkDb.Users.Single(u => u.UserId == "3");
            var checkRequest = checkDb.Requests.Single(u => u.RequestId == "002");

            //Check that the user and request were created correctly
            Assert.Equal("3", checkUser.UserId);
            Assert.Equal("testuser3", checkUser.Username);
            Assert.Equal("testuser3@localhost", checkUser.EmailAddress);
            Assert.Equal("0", checkUser.RoleId);

            //Note to self: I need to find out what is causing the CreatedByUserId value to not update in the database despite being set here. It could be trivial or not.
            //Date checking can be done in a different manner.
            Assert.Equal("002", checkRequest.RequestId);
            Assert.Equal("Test Service Request", checkRequest.SupportRequestTitle);
            Assert.Equal("This is a test service request", checkRequest.SupportRequestContent);
            Assert.False(checkRequest.IsResolved);
            Assert.Equal(RestApiServer.CommonEnums.TriageStatus.Untriaged, checkRequest.TriageStatus);
            Assert.Equal(RestApiServer.CommonEnums.TriageType.Unspecified, checkRequest.TriageType);
        }
    }
}