using RestApiServer.Db;
using RestApiServer.Dto.App;

namespace RestApiServer.Dto.Forum
{
    public class BoardBasicInfo
    {
        public required BoardEntry Board { get; set; } = null!;
        public required UserBasicInfo CreatedByUser { get; set; } = null!;
        public int NumTopics { get; set; } = 0;
    }

    public class BoardFullInfo
    {
        public required BoardEntry Board { get; set; } = null!;
        public required UserBasicInfo CreatedByUser { get; set; } = null!;
        public List<TopicBasicInfo> Topics { get; set; } = new();
        public required int TotalTopics { get; set; } = 0;
    }
    public class BoardSummary
    {
        public int TotalBoards { get; set; } = 0;
        public int BoardsPerCategory { get; set; } = 0;
    }
}