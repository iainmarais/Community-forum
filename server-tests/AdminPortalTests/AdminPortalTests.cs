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
            var testServiceRequest = new RequestEntry
            {
                RequestId = "002",
                CreatedByUserId = "1",
                SupportRequestTitle = "Test Service Request",
                SupportRequestContent = "This is a test service request",
                CreatedDate = DateTime.Now,
                IsResolved = false,
                TriageStatus = RestApiServer.CommonEnums.TriageStatus.Untriaged,
                TriageType = RestApiServer.CommonEnums.TriageType.Unspecified,
            };
            db.Add(testServiceRequest);
            db.SaveChanges();

            using var TestDb = new AppDbContext();

            Assert.Equal("002", TestDb.Requests.Single(u => u.RequestId == "002").RequestId);
            Assert.Equal("1", TestDb.Requests.Single(u => u.RequestId == "002").CreatedByUserId);
            Assert.Equal("Test Service Request", TestDb.Requests.Single(u => u.RequestId == "002").SupportRequestTitle);
            Assert.Equal("This is a test service request", TestDb.Requests.Single(u => u.RequestId == "002").SupportRequestContent);
            Assert.Equal(DateTime.Now, TestDb.Requests.Single(u => u.RequestId == "002").CreatedDate);
            Assert.False(TestDb.Requests.Single(u => u.RequestId == "002").IsResolved);
            Assert.Equal(RestApiServer.CommonEnums.TriageStatus.Untriaged, TestDb.Requests.Single(u => u.RequestId == "002").TriageStatus);
            Assert.Equal(RestApiServer.CommonEnums.TriageType.Unspecified, TestDb.Requests.Single(u => u.RequestId == "002").TriageType);
        }
    }
}