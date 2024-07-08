namespace RestApiServer.Dto.Forum
{
    public class CreateThreadRequest
    {
        public required string TopicId { get; set; }
        public required string ThreadName { get; set; }
        public required string CreatedByUserId { get; set; }
    }
}

namespace RestApiServer.Dto.Forum
{
    public class CreateThreadWithPostRequest
    {
        public required string TopicId { get; set; }
        public required string ThreadName { get; set; }
        public required string CreatedByUserId { get; set; }
        public required string MessageContent { get; set; }
    }
}