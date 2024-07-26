using RestApiServer.Db;
using RestApiServer.Db.Users;
using RestApiServer.Utils;

namespace RestApiServer.Dto.App
{
    public class PostBasicInfo : PostEntry
    {
        public PostBasicInfo(PostEntry post)
        {
            ClassUtils.CopyFromBaseclass(this, post);
        }
    }

    public class PostFullInfo
    {
        public required PostBasicInfo Post { get; set; }
        public required UserBasicInfo CreatedByUser { get; set; }
    }
}