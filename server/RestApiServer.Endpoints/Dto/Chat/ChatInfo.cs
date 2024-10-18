using RestApiServer.Db;

namespace RestApiServer.Dto.Chat
{
    public class ChatBasicInfo 
    {
        public ChatEntry Chat { get; set; } = null!;
    }
    public class ChatFullInfo
    {
        public ChatEntry Chat { get; set; } = null!;
        public List<ChatMessageBasicInfo> Messages { get; set; } = null!;
    }
}