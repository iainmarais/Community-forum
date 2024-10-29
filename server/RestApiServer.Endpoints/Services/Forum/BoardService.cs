using Microsoft.EntityFrameworkCore;
using RestApiServer.Endpoints.ApiResponses;
using RestApiServer.Db;
using RestApiServer.Dto.App;
using RestApiServer.Dto.Forum;

namespace RestApiServer.Endpoints.Services.Forum.Boards
{

    public class BoardService
    {

        public static async Task<List<BoardBasicInfo>> GetForumBoardsAsync()
        {
            using var db = new AppDbContext();
            var boards = await (from board in db.Boards
                                join user in db.Users on board.CreatedByUserId equals user.UserId
                                select new BoardBasicInfo
                                {
                                    Board = board,
                                    CreatedByUser = new UserBasicInfo()
                                    {
                                        User = user
                                    }
                                }).ToListAsync();
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
            if (board == null)
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
                    })
                    .ToList(),
                TotalTopics = board.TopicsCreated.Count
            };
            return boardFullInfo ?? throw new Exception("Board not found");
        }
        public static async Task<PaginatedData<List<TopicBasicInfo>, TopicSummary>> GetTopicsForBoardAsync(string boardId, int pageNumber, int rowsPerPage, string? searchTerm)
        {
            using var db = new AppDbContext();

            var topicsQuery = from topic in db.Topics
                              where topic.BoardId == boardId
                              join user in db.Users on topic.CreatedByUserId equals user.UserId
                              select new TopicBasicInfo
                              {
                                  Topic = topic,
                                  TotalPosts = topic.Threads.Sum(t => t.Posts.Count),
                                  TotalThreads = topic.Threads.Count,
                                  CreatedByUser = new UserBasicInfo()
                                  {
                                      User = user
                                  }
                              };

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    searchTerm = searchTerm.ToLower();
                    topicsQuery = from t in topicsQuery
                                    where t.Topic.TopicName.ToLower().Contains(searchTerm)
                                    || t.CreatedByUser!.User.UserFirstname.ToLower().Contains(searchTerm)
                                    || t.CreatedByUser!.User.UserLastname.ToLower().Contains(searchTerm)
                                    || t.CreatedByUser!.User.Username.ToLower().Contains(searchTerm)
                                    || t.Topic.Description.ToLower().Contains(searchTerm)
                                    select t;
                }

            //Count the results asynchronously
            var filteredTotal = await topicsQuery.CountAsync();
            var skip = (pageNumber - 1) * rowsPerPage;

            //Take the results asynchronously
            var topicRows = await topicsQuery.Skip(skip).Take(rowsPerPage).ToListAsync();

            int totalPages = (filteredTotal + rowsPerPage - 1) / rowsPerPage;

            return new()
            {
                Rows = topicRows,
                PageNumber = pageNumber,
                RowsPerPage = rowsPerPage,
                TotalPages = totalPages,
                Summary = new()
                {
                    TotalTopics = filteredTotal
                }
            };
        }
    }
}
