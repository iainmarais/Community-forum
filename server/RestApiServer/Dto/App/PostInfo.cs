using RestApiServer.Db;
using RestApiServer.Db.Users;
using RestApiServer.Utils;

namespace RestApiServer.Dto.App
{
    public class PostBasicInfo
    {
        public required PostEntry Post { get; set; }
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