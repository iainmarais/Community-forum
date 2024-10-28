using RestApiServer.Db;
using RestApiServer.Dto.App;

namespace RestApiServer.Dto.Forum
{
    public class PostBasicInfo
    {
        public required PostEntry Post { get; set; }
        public required UserBasicInfo CreatedByUser { get; set; }
    }

    public class PostFullInfo
    {
        public required PostEntry Post { get; set; }
        public required UserBasicInfo CreatedByUser { get; set; }
    }

    public class PostSummary 
    {
        public required int TotalPosts { get; set; } = 0;
    }
}