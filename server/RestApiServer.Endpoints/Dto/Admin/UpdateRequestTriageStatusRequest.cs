using RestApiServer.CommonEnums;

namespace RestApiServer.Dto.Admin
{
    public class UpdateRequestTriageStatusRequest
    {
        public TriageType TriageType { get; set; }
        public TriageStatus TriageStatus { get; set; }
    }
}