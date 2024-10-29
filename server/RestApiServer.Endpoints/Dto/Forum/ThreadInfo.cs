using RestApiServer.Db;
using RestApiServer.Dto.App;

namespace RestApiServer.Dto.Forum
{
    public class ThreadBasicInfo
    {
        public required ThreadEntry Thread { get; set; } = null!;
        public int TotalPosts { get; set; } = 0;
        public PostBasicInfo NewestMessage { get; set; } = null!;
        public UserBasicInfo CreatedByUser { get; set; } = null!;
        public TopicBasicInfo? Topic { get; set; } 
    }

    public class ThreadFullInfo 
    {
        public required ThreadEntry Thread { get; set; } = null!;
        public required int TotalPosts { get; set; } = 0;
        public required List<PostBasicInfo> Posts { get; set; }
        //Represents the user that created the thread
        public required UserBasicInfo CreatedByUser { get; set; }
    }

    public class ThreadSummary
    {
        public required int TotalThreads { get; set; }
    }
}