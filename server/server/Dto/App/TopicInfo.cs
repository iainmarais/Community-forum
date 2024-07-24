using RestApiServer.Db;
using RestApiServer.Db.Users;
using RestApiServer.Utils;

namespace RestApiServer.Dto.App
{
    public class TopicBasicInfo : TopicEntry
    {
        public TopicBasicInfo(TopicEntry topic)
        {
            ClassUtils.CopyFromBaseclass(this, topic);
        }
        public int TotalThreads { get; set; } = 0;
        public int TotalPosts { get; set; } = 0;
        public int NumTotalThreads { get; set; }
        public int NumNewThreads { get; set; }
        public ThreadBasicInfo NewestThread { get; set; } = null!;
    }

    public class TopicFullInfo
    {
        public required TopicBasicInfo Topic { get; set; }
        public required int TotalThreads { get; set; }
        public required UserBasicInfo CreatedByUser { get; set; }
        public required List<ThreadBasicInfo> Threads { get; set; }
    }

    public class CreateTopicRequest
    {
        public required string CategoryId { get; set; }
        public required string TopicName { get; set; }
        public required string Description { get; set; } 
    }
}