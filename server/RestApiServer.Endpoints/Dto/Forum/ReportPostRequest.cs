namespace RestApiServer.Dto.Forum
{
    public class ReportPostRequest
    {
        public required string ThreadId { get; set; }
        public required string PostId { get; set; }
        public required string ReportReason { get; set; }
        public required string ReportedByUserId { get; set; }
    }
}