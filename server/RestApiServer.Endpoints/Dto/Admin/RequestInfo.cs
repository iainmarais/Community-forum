using RestApiServer.Db;
using RestApiServer.Db.Users;

namespace RestApiServer.Dto.Admin
{
    public class RequestBasicInfo
    {
        public required RequestEntry Request { get; set; }
    }
    public class RequestFullInfo
    {
        public required RequestEntry Request { get; set; }
        public UserEntry CreatedByUser { get; set; } = new();
        public UserEntry AssignedToUser { get; set; } = new();
        public UserEntry ResolvedByUser { get; set; } = new();
        public UserEntry LastUpdatedByUser { get; set; } = new();        
    }

    public class RequestSummary
    {
        public int TotalRequests { get; set; } = 0;
        public int NumResolvedRequests { get; set; } = 0;
        public int NumAssignedRequests { get; set; } = 0;
        public int NumPendingRequests { get; set; } = 0;
    }
}