namespace RestApiServer.Dto.Chat
{
    public class CreateChatRequest
    {
        public required string ChatId { get; set; }
        public required string ChatName { get; set; }
        public required string SecondParticipantUserId { get; set; }
    }

        public class CreateGroupChatRequest
    {
        public required string ChatId { get; set; }
        public required string ChatName { get; set; }
        public required string ChatGroupId { get; set; }
        public required string SecondParticipantUserId { get; set; }
    }
}