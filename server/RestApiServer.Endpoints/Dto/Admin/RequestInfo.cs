using RestApiServer.Db;

namespace RestApiServer.Dto.Admin
{
    public class RequestBasicInfo
    {
        public required RequestEntry Request { get; set; }
    }

    public class RequestSummary
    {
        public int TotalRequests { get; set; } = 0;
        public int NumResolvedRequests { get; set; } = 0;
        public int NumAssignedRequests { get; set; } = 0;
        public int NumPendingRequests { get; set; } = 0;
    }
}