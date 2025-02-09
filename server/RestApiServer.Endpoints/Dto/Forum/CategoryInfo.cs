using RestApiServer.Db;

namespace RestApiServer.Dto.Forum
{
    public class CategoryBasicInfo
    {
        public required CategoryEntry Category { get; set; } = null!;
        //We don't need to pull the boards here, just the count
        public required int TotalBoards { get; set; } = 0;
    }

    public class CategoryFullInfo
    {
        public required CategoryEntry Category { get; set; } = null!;
        public List<BoardBasicInfo> Boards { get; set; } = new();
        public required int TotalBoards { get; set; } = 0;
    }

    public class CategorySummary
    {
        public int TotalCategories { get; set; } = 0;
    }
}