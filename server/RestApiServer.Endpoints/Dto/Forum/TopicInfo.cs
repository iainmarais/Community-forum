using RestApiServer.Db;
using RestApiServer.Dto.App;

namespace RestApiServer.Dto.Forum
{
    public class TopicBasicInfo
    {
        public required TopicEntry Topic { get; set; }
        public required int TotalThreads { get; set; } = 0;
        public required int TotalPosts { get; set; } = 0;
        public required int NumTotalThreads { get; set; }
        public int NumNewThreads { get; set; }
        public ThreadBasicInfo NewestThread { get; set; } = null!;
        public UserBasicInfo CreatedByUser { get; set; } = null!;
    }

    public class TopicFullInfo
    {
        public required TopicEntry Topic { get; set; }
        public required int TotalThreads { get; set; } = 0;
        public required int TotalPosts { get; set; } = 0;
        public required int NumTotalThreads { get; set; }
        public int NumNewThreads { get; set; }
        public ThreadBasicInfo NewestThread { get; set; } = null!;
        public required UserBasicInfo CreatedByUser { get; set; }
        public required List<ThreadBasicInfo> Threads { get; set; }
    }
    public class TopicSummary 
    {
        public required int TotalTopics { get; set; } = 0;
    }
}