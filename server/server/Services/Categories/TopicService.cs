using Microsoft.EntityFrameworkCore;
using RestApiServer.Core.ApiResponses;
using RestApiServer.Db;
using RestApiServer.Db.Users;
using RestApiServer.Dto.App;
using RestApiServer.Dto.Forum;
using RestApiServer.Utils;

namespace RestApiServer.Services.Categories
{
    public class TopicService
    {
        public static async Task<TopicFullInfo> GetForumTopicFullInfoAsync(string topicId)
        {
            using var db = new AppDbContext();
            var topic = await db.Topics
            .Include(t => t.Threads.OrderByDescending(t => t.CreatedDate))
            .Include(t => t.CreatedByUser)
            .SingleOrDefaultAsync(t => t.TopicId == topicId);
            if(topic == null)
            {
                throw new Exception("Topic not found");
            }
            var topicFullInfo = new TopicFullInfo()
            {
                Topic = new TopicBasicInfo(topic),
                Threads = topic.Threads.Select(t => new ThreadBasicInfo(t)).ToList(),
                TotalThreads = topic.Threads.Count,
                CreatedByUser = new UserBasicInfo(topic.CreatedByUser ?? UserEntry.CreateDefaultGuestUser())
            };

            return topicFullInfo ?? throw new Exception("Topic not found");
        }
        public static async Task<PaginatedData<List<TopicBasicInfo>, TopicSummary>> GetForumTopicsAsync(int pageNumber,int rowsPerPage, string searchTerm)
        {
            using var db = new AppDbContext();
            var topics = db.Topics
                                .Select(t => new TopicBasicInfo(t))
                                .Include(t => t.CreatedByUser)
                                .AsEnumerable();
            var filteredTopics = topics;
            if(!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                filteredTopics = (from t in filteredTopics 
                                where t.TopicName.ToLower().Contains(searchTerm)
                                || t.Description.ToLower().Contains(searchTerm)
                                || t.CreatedByUser.Username.ToLower().Contains(searchTerm)
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

        public static async Task<List<TopicBasicInfo>> GetNewestForumTopicsAsync()
        {
            using var db = new AppDbContext();
            var res = await db.Topics
                                .OrderByDescending(t => t.CreatedDate)
                                .Take(10)
                                .Select(t => new TopicBasicInfo(t))
                                .ToListAsync();
            return res;
        }

        public static async Task<List<TopicBasicInfo>> GetPopularForumTopicsAsync()
        {
            /*
            How this should work:
                Iterate over all the threads and see which of them share the same topic id. 
                Threshold for popularity is 5 threads per topic. It should be a range of values from 5 to 20.
                Return the found topic entries that match this threshold.
            */
            using var db = new AppDbContext();

            //Get all the topic ids that have at least 5 and at most 20 threads.
            var popularTopics = await db.Threads
                                    .GroupBy(t => t.TopicId)
                                    .Where(g => g.Count() >= 5 && g.Count() <= 20)
                                    .Select(g => g.Key)
                                    .ToListAsync();

            //Get the topic entries that match the found topic ids.
            var res = await db.Topics
                                .Where(t => popularTopics.Contains(t.TopicId))
                                .Select(t => new TopicBasicInfo(t))
                                .ToListAsync();
            //Return the result.
            return res;
        }

        public static async Task<PaginatedData<List<TopicBasicInfo>, TopicSummary>> CreateForumTopicAsync(string userId, CreateTopicRequest request)
        {
            using var db = new AppDbContext();
            //Check if a topic with the name already exists. This will help avoid creating duplicates.
            if(await db.Topics.AnyAsync(t => t.TopicName == request.TopicName))
            {
                throw new Exception("Topic already exists");
            }
            var topic = new TopicEntry
            {
                TopicId = DbUtils.GenerateUuid(),
                BoardId = request.BoardId,
                TopicName = request.TopicName,
                Description = request.Description,
                CreatedByUserId = userId,
                CreatedDate = DateTime.Now
            };
            db.Topics.Add(topic);
            await db.SaveChangesAsync();
            return await GetForumTopicsAsync(1, 10, "");
        }       
    }
}