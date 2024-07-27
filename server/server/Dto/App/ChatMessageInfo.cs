using RestApiServer.Db;
using RestApiServer.Utils;

namespace RestApiServer.Dto.App
{
    public class ChatMessageBasicInfo : ChatMessageEntry
    {
        public ChatMessageBasicInfo(ChatMessageEntry message)
        {
            ClassUtils.CopyFromBaseclass(this, message);
        }
    }
}