using RestApiServer.Db;
using RestApiServer.Db.Users;
using RestApiServer.Utils;

namespace RestApiServer.Dto.App
{
    public class CategoryBasicInfo : CategoryEntry
    {
        public CategoryBasicInfo(CategoryEntry category)
        {
            ClassUtils.CopyFromBaseclass(this, category);
        }
        public List<BoardBasicInfo> Boards { get; set; } = new();
    }

    public class CategoryFullInfo
    {
        public required CategoryBasicInfo Category { get; set; } = null!;
        public required List<BoardBasicInfo> Boards { get; set; } = new();
        public required int TotalBoards { get; set; } = 0;
    }
}