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
            var category = await db.Categories.SingleAsync(c => c.CategoryId == categoryId);
            var boards = await (from b in db.Boards
                                where b.CategoryId == category.CategoryId
                                join user in db.Users on b.CreatedByUserId equals user.UserId
                                select new BoardBasicInfo()
                                {
                                    Board = b,
                                    CreatedByUser = new UserBasicInfo()
                                    {
                                        User = user
                                    }
                                }).ToListAsync();
            var categoryFullInfo = new CategoryFullInfo()
            {
                Category = category,
                Boards = boards,
                TotalBoards = boards.Count()
            };
            return categoryFullInfo ?? throw new Exception("Category not found");
        }

        public static async Task<List<CategoryBasicInfo>> GetForumCategoriesAsync()
        {
            using var db = new AppDbContext();
            var categories = await (from category in db.Categories
                                    select new CategoryBasicInfo()
                                    {
                                        Category = category,
                                        Boards = (from b in db.Boards
                                                where b.CategoryId == category.CategoryId
                                                join user in db.Users on b.CreatedByUserId equals user.UserId
                                                select new BoardBasicInfo()
                                                {
                                                    Board = b,
                                                    CreatedByUser = new UserBasicInfo()
                                                    {
                                                        User = user
                                                    }
                                                }).AsEnumerable().ToList()
                                    }).ToListAsync();
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
            var boards = from board in db.Boards
                            where board.CategoryId == category.CategoryId
                            join user in db.Users on board.CreatedByUserId equals user.UserId
                            orderby board.BoardName descending 
                            select new BoardBasicInfo()
                            {
                                Board = board,
                                CreatedByUser = new UserBasicInfo()
                                {
                                    User = user
                                }
                            };            
            return new CategoryBasicInfo()
            {
                Category = category,
                Boards = boards.ToList()
            };
        }
    }
}