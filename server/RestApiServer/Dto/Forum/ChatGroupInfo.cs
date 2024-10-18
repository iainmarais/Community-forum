using RestApiServer.Common.Utils;
using RestApiServer.Db;

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