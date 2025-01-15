using RestApiServer.Db;
using RestApiServer.Dto.App;

namespace RestApiServer.Dto.Forum
{
    /// <summary>
    /// Represents the DTO sent back from the backend to the forum frontend. 
    /// The UserBasicInfo dataset is included for primarily setting the user information on the found requests, 
    /// but can also be used for sorting and filtering.
    /// </summary>
    public class RequestBasicInfo
    {
        public required RequestEntry Request {get; set; }
        //Needed to filter out any requests where CreatedByUserId is the same as CreatedByUser.UserId.
        public required UserBasicInfo CreatedByUser { get; set; }
    }
    public class RequestSummary
    {
        public int TotalRequests { get; set; } = 0;
        public int NumResolvedRequests { get; set; } = 0;
        public int NumPendingRequests { get; set; } = 0;
    }
}