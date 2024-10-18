using RestApiServer.Db;
using RestApiServer.Dto.App;

namespace RestApiServer.Dto.Forum
{
    public class BoardBasicInfo
    {
        public required BoardEntry Board { get; set; } = null!;
        public required UserBasicInfo CreatedByUser { get; set; } = null!;
    }

    public class BoardFullInfo
    {
        public required BoardEntry Board { get; set; } = null!;
        public required List<TopicBasicInfo> Topics { get; set; } = new();
        public required int TotalTopics { get; set; } = 0;
    }
}