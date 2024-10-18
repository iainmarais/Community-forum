namespace RestApiServer.Dto.Forum
{
    public class CreateTopicRequest
    {
        public required string BoardId { get; set; }
        public required string TopicName { get; set; }
        public required string Description { get; set; } 
    }
}