using Microsoft.EntityFrameworkCore;
using RestApiServer.Db;
using RestApiServer.Dto.App;

namespace RestApiServer.Services.Categories
{

    public class CategoryService
    {

        public static async Task<CategoryFullInfo> GetForumCategoryFullInfoAsync(string categoryId)
        {
            using var db = new AppDbContext();
            var category = await db.Categories
            .Include(c => c.TopicsCreated.OrderByDescending(t => t.CreatedDate))
            .SingleOrDefaultAsync(c => c.CategoryId == categoryId);
            if(category == null)
            {
                throw new Exception("Category not found");
            }
            var categoryFullInfo = new CategoryFullInfo()
            {
                Category = new CategoryBasicInfo(category),
                Topics = category.TopicsCreated
                    .Select(t => new TopicBasicInfo(t)
                    {
                        TotalThreads = t.Threads.Count,
                        TotalPosts = t.Threads.Sum(t => new ThreadBasicInfo(t).TotalPosts)
                    })
                    .ToList(),
                TotalTopics = category.TopicsCreated.Count
            };
            return categoryFullInfo ?? throw new Exception("Category not found");
        }

        public static async Task<List<CategoryBasicInfo>> GetForumCategoriesAsync()
        {
            using var db = new AppDbContext();
            var categories = await db.Categories
            .Include(c => c.TopicsCreated.OrderByDescending(t => t.CreatedDate))
            .Select(c => new CategoryBasicInfo(c)
            {
                Topics = c.TopicsCreated
                    .Select(t => new TopicBasicInfo(t))
                    .ToList()
            })
            .ToListAsync();
            return categories;
        }
    }
}