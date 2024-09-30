using RestApiServer.Db;
using RestApiServer.Utils;

namespace RestApiServer.Dto.App
{
    public class ChatGroupBasicInfo : ChatGroupEntry
    {
        public ChatGroupBasicInfo(ChatGroupEntry chatGroup)
        {
            ClassUtils.CopyFromBaseclass(this, chatGroup);
        }
    }
}