namespace RestApiServer.Endpoints.Dto.Admin
{
    public class AdminCreateThreadRequest
    {
        public required string ThreadName { get; set; }
        public required string TopicId { get; set; }
    }
}