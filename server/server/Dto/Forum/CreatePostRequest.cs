namespace RestApiServer.Dto.Forum
{
    public class CreatePostRequest
    {
        public required string ThreadId { get; set; }
        public required bool IsFirstPost { get; set; }
        public required string CreatedByUserId { get; set; }
        public required string PostContent { get; set; } = string.Empty;
        public string ReplyToPostId { get; set; } = string.Empty;
    }
}