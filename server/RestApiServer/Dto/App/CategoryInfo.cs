using RestApiServer.Db;
using RestApiServer.Db.Users;
using RestApiServer.Utils;

namespace RestApiServer.Dto.App
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
}