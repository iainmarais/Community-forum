using RestApiServer.Db;
using RestApiServer.Utils;

namespace RestApiServer.Dto.App
{
    public class ChatMessageBasicInfo
    {
        public required ChatMessageEntry Message { get; set; } = null!;
    }
}