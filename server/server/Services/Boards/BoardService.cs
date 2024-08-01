using Microsoft.EntityFrameworkCore;
using RestApiServer.Core.ApiResponses;
using RestApiServer.Db;
using RestApiServer.Db.Users;
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
            .Select(b => new BoardBasicInfo()
            {
                Board = b,
            })
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
                Board = new BoardEntry()
                {
                    BoardId = board.BoardId,
                    BoardName = board.BoardName,
                    BoardDescription = board.BoardDescription,
                    CategoryId = board.CategoryId,
                    CreatedByUserId = board.CreatedByUserId
                },
                Topics = board.TopicsCreated
                    .Select(t => new TopicBasicInfo()
                    {
                        Topic = t,
                        TotalPosts = t.Threads.Sum(t => t.Posts.Count),
                        TotalThreads = t.Threads.Count,
                        NumTotalThreads = t.Threads.Count
                    })
                    .ToList(),
                TotalTopics = board.TopicsCreated.Count
            };
            return boardFullInfo ?? throw new Exception("Board not found");
        }
        public static async Task<PaginatedData<List<TopicBasicInfo>, TopicSummary>> GetAssociatedBoardTopicsAsync(string boardId, int pageNumber, int rowsPerPage, string searchTerm)
        {
            using var db = new AppDbContext();
            var topics = db.Topics
                    .Where(t => t.BoardId == boardId)
                    .Select(t => new TopicBasicInfo()
                    {
                        Topic = t,
                        TotalPosts = t.Threads.Sum(t => t.Posts.Count),
                        TotalThreads = t.Threads.Count,
                        NumTotalThreads = t.Threads.Count,
                    })
                    .Include(t => t.CreatedByUser)
                    .AsEnumerable();
            var filteredTopics = topics;
            if(!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                filteredTopics = (from t in filteredTopics 
                                where t.Topic.TopicName.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase)
                                || t.Topic.Description.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase)
                                || t.CreatedByUser!.Username.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase)
                                  select t);
            }
            var filteredTotal = filteredTopics.Count();
            var skip = (pageNumber - 1) * rowsPerPage;
            var pageRows = filteredTopics.Skip(skip).Take(rowsPerPage);
            var topicRows = new List<TopicBasicInfo>();
            foreach (var topic in pageRows)
            {
                topicRows.Add(topic);
            }
            int totalPages = 1;
            if(filteredTotal > rowsPerPage)
            {
                totalPages = filteredTotal / rowsPerPage;
            }
            return new()
            {
                Rows = topicRows,
                RowsPerPage = rowsPerPage,
                PageNumber = pageNumber,
                TotalPages = totalPages,
                Summary = new()
                {
                    TotalTopics = filteredTotal
                }
            };            
        }
    }
}