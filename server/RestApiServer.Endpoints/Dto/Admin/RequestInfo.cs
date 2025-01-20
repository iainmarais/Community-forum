using RestApiServer.Db;
using RestApiServer.Db.Users;
using RestApiServer.Dto.App;

namespace RestApiServer.Dto.Admin
{
    public class RequestBasicInfo
    {
        public required RequestEntry Request { get; set; }
        public UserBasicInfo CreatedByUser { get; set; } = null!;
    }
    public class RequestFullInfo
    {
        public required RequestEntry Request { get; set; }
        public UserBasicInfo CreatedByUser { get; set; } = null!;
        public UserBasicInfo AssignedToUser { get; set; } = null!;
        public UserBasicInfo ResolvedByUser { get; set; } = null!;
        public UserBasicInfo LastUpdatedByUser { get; set; } = null!;   
    }

    public class RequestSummary
    {
        public int TotalRequests { get; set; } = 0;
        public int NumResolvedRequests { get; set; } = 0;
        public int NumAssignedRequests { get; set; } = 0;
        public int NumPendingRequests { get; set; } = 0;
    }
}