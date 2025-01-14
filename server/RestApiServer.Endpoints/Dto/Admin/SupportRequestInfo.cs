using RestApiServer.Db;

namespace RestApiServer.Dto.Admin
{
    public class SupportRequestBasicInfo
    {
        public required SupportRequestEntry SupportRequest { get; set; }
    }

    public class SupportRequestSummary
    {
        public int TotalSupportRequests { get; set; } = 0;
        public int NumResolvedRequests { get; set; } = 0;
        public int NumAssignedRequests { get; set; } = 0;
        public int NumPendingRequests { get; set; } = 0;
    }
}