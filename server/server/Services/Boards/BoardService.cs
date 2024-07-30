using Microsoft.EntityFrameworkCore;
using RestApiServer.Db;
using RestApiServer.Dto.App;
using RestApiServer.Dto.Forum;

namespace RestApiServer.Services.Boards
{

    public class BoardService
    {

        public static async Task<List<BoardBasicInfo>> GetForumBoardsAsync()
        {
            using var db = new AppDbContext();
            var boards = await db.Boards
            .Select(b => new BoardBasicInfo(b))
            .ToListAsync();
            return boards;
        }

        public static async Task<List<BoardBasicInfo>> CreateForumBoardAsync(string userId, CreateBoardRequest request)
        {
            using var db = new AppDbContext();
            var board = new BoardEntry
            {
                BoardId = Guid.NewGuid().ToString(),
                CreatedByUserId = userId,
                BoardName = request.BoardName,
                BoardDescription = request.BoardDescription,
                CategoryId = request.CategoryId
            };
            await db.Boards.AddAsync(board);
            await db.SaveChangesAsync();
            return await GetForumBoardsAsync();
        }

        public static async Task<BoardFullInfo> GetSelectedBoardAsync(string boardId)
        {
            using var db = new AppDbContext();
            var board = await db.Boards
            .Include(b => b.TopicsCreated.OrderByDescending(t => t.TopicName))
            .SingleOrDefaultAsync(b => b.BoardId == boardId);
            if(board == null)
            {
                throw new Exception("Board not found");
            }
            var boardFullInfo = new BoardFullInfo()
            {
                Board = new BoardBasicInfo(board),
                Topics = board.TopicsCreated
                    .Select(t => new TopicBasicInfo(t))
                    .ToList(),
                TotalTopics = board.TopicsCreated.Count
            };
            return boardFullInfo ?? throw new Exception("Board not found");
        }
    }
}