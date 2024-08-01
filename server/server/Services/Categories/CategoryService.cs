using Microsoft.EntityFrameworkCore;
using RestApiServer.Db;
using RestApiServer.Dto.App;
using RestApiServer.Dto.Forum;

namespace RestApiServer.Services.Categories
{

    public class CategoryService
    {

        public static async Task<CategoryFullInfo> GetForumCategoryFullInfoAsync(string categoryId)
        {
            using var db = new AppDbContext();
            var category = await db.Categories
            .Include(c => c.BoardsCreated.OrderByDescending(b => b.BoardName))
            .SingleOrDefaultAsync(b => b.CategoryId == categoryId);
            if(category == null)
            {
                throw new Exception("Category not found");
            }
            var categoryFullInfo = new CategoryFullInfo()
            {
                Category = category,
                Boards = category.BoardsCreated
                    .Select(b => new BoardBasicInfo()
                    {
                        Board = b
                    }).ToList(),
                TotalBoards = category.BoardsCreated.Count
            };
            return categoryFullInfo ?? throw new Exception("Category not found");
        }

        public static async Task<List<CategoryBasicInfo>> GetForumCategoriesAsync()
        {
            using var db = new AppDbContext();
            var categories = await db.Categories
            .Include(c => c.BoardsCreated.OrderByDescending(b => b.BoardName))
            .Select(c => new CategoryBasicInfo()
            {
                Category = c,
                Boards = c.BoardsCreated
                    .Select(b => new BoardBasicInfo()
                    {
                        Board = b
                    })
                    .ToList()
            })
            .ToListAsync();
            return categories;
        }

        public static async Task<List<CategoryBasicInfo>> CreateForumCategoryAsync(CreateCategoryRequest request)
        {
            using var db = new AppDbContext();
            var category = new CategoryEntry
            {
                CategoryId = Guid.NewGuid().ToString(),
                CategoryName = request.CategoryName,
                CategoryDescription = request.CategoryDescription
            };
            db.Categories.Add(category);
            await db.SaveChangesAsync();
            return await GetForumCategoriesAsync();
        }

        public static async Task<CategoryBasicInfo> GetSelectedCategoryAsync(string categoryId)
        {
            using var db = new AppDbContext();
            var category = await db.Categories
            .Include(c => c.BoardsCreated.OrderByDescending(b => b.BoardName))
            .SingleOrDefaultAsync(c => c.CategoryId == categoryId);
            if(category == null)
            {
                throw new Exception("Category not found");
            }
            return new CategoryBasicInfo()
            {
                Category = category,
                Boards = category.BoardsCreated
                    .Select(b => new BoardBasicInfo()
                    {
                        Board = b
                    })
                    .ToList()
            };
        }
    }
}