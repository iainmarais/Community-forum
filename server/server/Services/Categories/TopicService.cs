using Microsoft.EntityFrameworkCore;
using RestApiServer.Db;
using RestApiServer.Db.Users;
using RestApiServer.Dto.App;

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
        public static async Task<List<TopicBasicInfo>> GetForumTopicsAsync()
        {
            using var db = new AppDbContext();
            var res = await db.Topics
                                .Select(t => new TopicBasicInfo(t))
                                .ToListAsync();
            return res;
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
    }
}