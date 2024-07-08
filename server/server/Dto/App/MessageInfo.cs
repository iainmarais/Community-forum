using RestApiServer.Db;
using RestApiServer.Db.Users;
using RestApiServer.Utils;

namespace RestApiServer.Dto.App
{
    public class MessageBasicInfo : MessageEntry
    {
        public MessageBasicInfo(MessageEntry message)
        {
            ClassUtils.CopyFromBaseclass(this, message);
        }
    }

    public class MessageFullInfo
    {
        public required MessageBasicInfo Message { get; set; }
        public required UserBasicInfo CreatedByUser { get; set; }
    }
}