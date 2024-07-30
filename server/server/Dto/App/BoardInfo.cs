using RestApiServer.Db;
using RestApiServer.Utils;

namespace RestApiServer.Dto.App
{
    public class BoardBasicInfo : BoardEntry
    {
        public BoardBasicInfo(BoardEntry board)
        {   
            ClassUtils.CopyFromBaseclass(this, board);
        }
    }

    public class BoardFullInfo
    {
        public required BoardBasicInfo Board { get; set; } = null!;
        public required List<TopicBasicInfo> Topics { get; set; } = new();
        public required int TotalTopics { get; set; } = 0;
    }
}