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
                Topic = topic,
                Threads = topic.Threads.Select(t => new ThreadBasicInfo()
                {
                    Thread = t,
                    TotalPosts = t.Posts.Count,
                }).ToList(),
                TotalThreads = topic.Threads.Count,
                NumTotalThreads = topic.Threads.Count,
                TotalPosts = topic.Threads.Sum(t => t.Posts.Count),
                CreatedByUser = new UserBasicInfo(topic.CreatedByUser ?? UserEntry.CreateDefaultGuestUser())
            };

            return topicFullInfo ?? throw new Exception("Topic not found");
        }
    
        public static async Task<List<TopicBasicInfo>> GetNewestForumTopicsAsync()
        {
            using var db = new AppDbContext();
            var res = await db.Topics
                                .OrderByDescending(t => t.CreatedDate)
                                .Take(10)
                                .Select(t => new TopicBasicInfo()
                                {
                                    Topic = t,
                                    TotalPosts = t.Threads.Sum(t => t.Posts.Count),
                                    TotalThreads = t.Threads.Count,
                                    NumTotalThreads = t.Threads.Count,
                                })
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
                                .Include(t => t.CreatedByUser)
                                .Select(t => new TopicBasicInfo()
                                {
                                    Topic = t,
                                    TotalPosts = t.Threads.Sum(t => t.Posts.Count),
                                    TotalThreads = t.Threads.Count,
                                    NumTotalThreads = t.Threads.Count,
                                })
                                .ToListAsync();
            //Return the result.
            return res;
        }

        public static async Task<TopicFullInfo> CreateForumTopicAsync(string userId, CreateTopicRequest request)
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
            var associatedThreads = db.Threads.Where(t => t.TopicId == topic.TopicId);
            var res = new TopicFullInfo()
            {
                Topic = topic,
                Threads = topic.Threads.Select(t => new ThreadBasicInfo()
                {
                    Thread = t,
                    TotalPosts = t.Posts.Count,
                }).ToList(),
                TotalThreads = topic.Threads.Count,
                NumTotalThreads = topic.Threads.Count,
                TotalPosts = topic.Threads.Sum(t => t.Posts.Count),
                CreatedByUser = new UserBasicInfo(topic.CreatedByUser ?? UserEntry.CreateDefaultGuestUser())
            };
            return res;
        }

        public static async Task<PaginatedData<List<ThreadBasicInfo>, ThreadSummary>> GetPaginatedThreadsForTopicAsync(string topicId, int pageNumber, int rowsPerPage, string? searchTerm)
        {
            using var db = new AppDbContext();
            var threads = (from t in db.Threads
                            where t.TopicId == topicId
                            join u in db.Users on t.CreatedByUserId equals u.UserId
                            select new ThreadEntry()
                            {
                                CreatedByUser = new UserBasicInfo(u),
                                ThreadId = t.ThreadId,
                                TopicId = t.TopicId,
                                ThreadName = t.ThreadName,
                                CreatedDate = t.CreatedDate,
                                CreatedByUserId = t.CreatedByUserId
                            })
                            .AsEnumerable();
            var filteredThreads = threads;
            if(!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                filteredThreads = (from t in filteredThreads 
                                where t.ThreadName.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase)
                                || t.CreatedByUser.UserFirstname.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase)
                                || t.CreatedByUser.UserLastname.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase)
                                || t.CreatedByUser!.Username.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase)
                                  select t);
            }
            var filteredTotal = filteredThreads.Count();
            var skip = (pageNumber - 1) * rowsPerPage;
            var pageRows = filteredThreads.Skip(skip).Take(rowsPerPage);
            var threadRows = new List<ThreadBasicInfo>();
            foreach (var thread in pageRows)
            {
                threadRows.Add(new ThreadBasicInfo() 
                {
                    Thread = thread,
                    TotalPosts = thread.Posts.Count,
                    CreatedByUser = new UserBasicInfo(thread.CreatedByUser ?? UserEntry.CreateDefaultGuestUser())
                });
            }
            int totalPages = 1;
            if(filteredTotal > rowsPerPage)
            {
                totalPages = filteredTotal / rowsPerPage;
            }
            return new()
            {
                Rows = threadRows,
                PageNumber = pageNumber,
                RowsPerPage = rowsPerPage,
                TotalPages = totalPages,
                Summary = new()
                {
                    TotalThreads = filteredTotal
                }
            };
        }      
    }
}