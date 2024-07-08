using RestApiServer.Db;
using RestApiServer.Db.Users;
using RestApiServer.Utils;

namespace RestApiServer.Dto.App
{
    public class ThreadBasicInfo : ThreadEntry
    {
        public ThreadBasicInfo(ThreadEntry thread)
        {
            ClassUtils.CopyFromBaseclass(this, thread);
        }
        public string CreatedByUserName { get; set; } = "";
        public string CreatedByUserFirstname { get; set; } = "";
        public string CreatedByUserLastname { get; set; } = "";
        public int TotalPosts { get; set; } = 0;
        public MessageBasicInfo NewestMessage { get; set; } = null!;
    }

    public class ThreadFullInfo 
    {
        public required ThreadBasicInfo Thread { get; set; }
        public required List<MessageBasicInfo> Messages { get; set; }
        //Represents the user that created the thread
        public required UserBasicInfo CreatedByUser { get; set; }
    }
}