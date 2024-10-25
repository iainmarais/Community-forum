using RestApiServer.Db;

namespace RestApiServer.Dto.Forum
{
    public class CategoryBasicInfo
    {
        public required CategoryEntry Category { get; set; } = null!;
        public required List<BoardBasicInfo> Boards { get; set; } = new();
    }

    public class CategoryFullInfo
    {
        public required CategoryEntry Category { get; set; } = null!;
        public required List<BoardBasicInfo> Boards { get; set; } = new();
        public required int TotalBoards { get; set; } = 0;
    }

    public class CategorySummary
    {
        public int TotalCategories { get; set; } = 0;
    }
}