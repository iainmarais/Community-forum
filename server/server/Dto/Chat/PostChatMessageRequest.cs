namespace RestApiServer.Dto.Chat
{
    public class PostChatMessageRequest
    {
        public string Message { get; set; } = "";
        public string ChatId { get; set; } = ""; // What chat session is this message being posted to?
        public string RecipientId { get; set; } = "";
    }
}