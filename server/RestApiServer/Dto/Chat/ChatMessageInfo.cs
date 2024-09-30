using RestApiServer.Db;
using RestApiServer.Dto.App;
using RestApiServer.Utils;

namespace RestApiServer.Dto.Chat
{
    public class ChatMessageBasicInfo
    {
        public required ChatMessageEntry Message { get; set; } = null!;
    }

    public class ChatMessageFullInfo
    {
        public required ChatMessageEntry Message { get; set; } = null!;
        public required UserBasicInfo Sender { get; set; } //This is the user who sent the message
        public required UserBasicInfo Recipient { get; set; }
    }
}