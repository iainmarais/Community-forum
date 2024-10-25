namespace RestApiServer.Endpoints.Dto.Admin
{
    public class CreateTopicRequest
    {
        public string BoardId { get; set; } = "";
        public string TopicName { get; set; } = "";
        public string Description { get; set; } = "";
        //Other props like CreatedDate, CreatedByUserId, and TopicId will be populated by the server. The CreatedByUserId will be the user id pulled from the user context.
    }
}