using RestApiServer.Db.Users;
using RestApiServer.Dto.App;

namespace RestApiServer.Dto.Forum
{
    public class ForumAppState
    {
        public required ForumStats ForumStats { get; set; }
        public LoggedInUserInfo? LoggedInUser { get; set; }
    }

    public class ForumStats
    {
        public int TotalPosts { get; set; } = 0;
        public int TotalUsers { get; set; } = 0;
        public int TotalThreads { get; set; } = 0;
        public int TotalTopics { get; set; } = 0;
        public List<UserBasicInfo>? MostActiveUsers { get; set; } = null;
        public int PopularTopics { get; set; } = 0;
    }
}