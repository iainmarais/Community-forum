using Microsoft.EntityFrameworkCore;
using RestApiServer.Core.Errorhandler;
using RestApiServer.Db;
using RestApiServer.Dto.App;
using RestApiServer.Dto.Forum;
using RestApiServer.Endpoints.ApiResponses;
using RestApiServer.Endpoints.Dto.Admin;

namespace RestApiServer.Endpoints.Services.Admin
{
    public class ContentManagementService
    {
       public static async Task CreateCategoryAsync(string userId, AdminCreateCategoryRequest req)
        {
            using var db = new AppDbContext();

            var category = new CategoryEntry()
            {
                CategoryId = Guid.NewGuid().ToString(),
                CategoryName = req.CategoryName,
                CategoryDescription = req.CategoryDescription,
                CreatedByUserId = userId
            };

            db.Categories.Add(category);

            await db.SaveChangesAsync();
        }

        public static async Task CreateBoardAsync(string userId, AdminCreateBoardRequest req)
        {
            using var db = new AppDbContext();

            var board = new BoardEntry()
            {
                BoardId = Guid.NewGuid().ToString(),
                BoardName = req.BoardName,
                BoardDescription = req.BoardDescription,
                CategoryId = req.CategoryId,
                CreatedByUserId = userId
            };

            db.Boards.Add(board);

            await db.SaveChangesAsync();
        }

        public static async Task CreateTopicAsync(string userId, AdminCreateTopicRequest req)
        {
            using var db = new AppDbContext();

            var topic = new TopicEntry()
            {
                TopicId = Guid.NewGuid().ToString(),
                BoardId = req.BoardId,
                TopicName = req.TopicName,
                Description = req.Description,
                CreatedByUserId = userId,
                CreatedDate = DateTime.UtcNow
            };

            db.Topics.Add(topic);

            await db.SaveChangesAsync();
        }

        public static async Task<PaginatedData<List<CategoryBasicInfo>, CategorySummary>> GetCategoriesAsync(int pageNumber, int rowsPerPage, string searchTerm = "")
        {
            using var db = new AppDbContext();

            var associatedBoardsQuery = from b in db.Boards 
                                        join u in db.Users on b.CreatedByUserId equals u.UserId
                                        orderby b.BoardName descending
                                        select new BoardBasicInfo
                                        {
                                            Board = b,
                                            CreatedByUser = new UserBasicInfo
                                            {
                                                User = u
                                            }
                                        };

            var categoriesQuery = from c in db.Categories
                                join u in db.Users on c.CreatedByUserId equals u.UserId
                                orderby c.CategoryName descending
                                select new CategoryBasicInfo
                                {
                                    Category = c,
                                    Boards = associatedBoardsQuery.Where(b => c.CategoryId == b.Board.CategoryId).ToList(),
                                    /* Not yet available on categories.
                                    CreatedByUser = new UserBasicInfo
                                    {
                                        User = u
                                    }
                                    */
                                };
                            
            if(!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                categoriesQuery = (from c in categoriesQuery
                                where c.Category.CategoryName.ToLower().Contains(searchTerm) ||
                                    c.Category.CategoryDescription.ToLower().Contains(searchTerm) ||
                                    c.Boards.Any(b => b.Board.BoardName.ToLower().Contains(searchTerm)) ||
                                    c.Boards.Any(b => b.Board.BoardDescription.ToLower().Contains(searchTerm)) ||
                                    c.Boards.Any(b => b.CreatedByUser.User.Username.ToLower().Contains(searchTerm))
                                select c);
            }
            var filteredTotal = await categoriesQuery.CountAsync();
            
            var skip = (pageNumber - 1) * rowsPerPage;
            int totalPages = (filteredTotal + rowsPerPage - 1) / rowsPerPage;

            var categoryRows = await categoriesQuery
                .Skip(skip)
                .Take(rowsPerPage)
                .ToListAsync();
            
            return new()
            {
                Rows = categoryRows,
                PageNumber = pageNumber,
                RowsPerPage = rowsPerPage,
                TotalPages = totalPages,
                Summary = new()
                {
                    TotalCategories = filteredTotal
                }
            };
        }

        public static async Task<PaginatedData<List<BoardBasicInfo>, BoardSummary>> GetBoardsAsync(int pageNumber, int rowsPerPage, string searchTerm = "")
        {
            using var db = new AppDbContext();
            var associatedPostsQuery = from p in db.Posts 
                                        join u in db.Users on p.CreatedByUserId equals u.UserId
                                        select new PostBasicInfo
                                        {
                                            Post = p,
                                            CreatedByUser = new UserBasicInfo
                                            {
                                                User = u
                                            }
                                        };

            var associatedThreadsQuery = from th in db.Threads
                                        join u in db.Users on th.CreatedByUserId equals u.UserId
                                        select new ThreadBasicInfo
                                        {
                                            Thread = th,
                                            CreatedByUser = new UserBasicInfo
                                            {
                                                User = u
                                            },
                                            TotalPosts = associatedPostsQuery.Where(p => p.Post.ThreadId == th.ThreadId).Count()
                                        };

            var associatedTopicsQuery = from t in db.Topics
                                        join u in db.Users on t.CreatedByUserId equals u.UserId
                                        select new TopicBasicInfo
                                        {
                                            Topic = t,
                                            CreatedByUser = new UserBasicInfo
                                            {
                                                User = u
                                            },
                                            TotalThreads = associatedThreadsQuery.Where(th => th.Thread.TopicId == t.TopicId).Count(),
                                            TotalPosts = associatedThreadsQuery.Where(th => th.Thread.TopicId == t.TopicId).Sum(th => th.TotalPosts)
                                        };

            var boardsQuery = from b in db.Boards 
                            join u in db.Users on b.CreatedByUserId equals u.UserId
                            select new BoardBasicInfo
                            {
                                Board = b,
                                CreatedByUser = new UserBasicInfo()
                                { 
                                    User = u
                                },
                                Topics = associatedTopicsQuery.ToList()
                            };
            if(!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                boardsQuery = (from b in boardsQuery
                            where b.Board.BoardName.ToLower().Contains(searchTerm) ||
                                b.Board.BoardDescription.ToLower().Contains(searchTerm) ||
                                b.CreatedByUser.User.Username.ToLower().Contains(searchTerm) ||
                                b.CreatedByUser.User.UserLastname.ToLower().Contains(searchTerm) ||
                                b.CreatedByUser.User.UserFirstname.ToLower().Contains(searchTerm) ||
                                b.CreatedByUser.User.EmailAddress.ToLower().Contains(searchTerm)
                            select b);
            }
            var filteredTotal = await boardsQuery.CountAsync();
            
            var skip = (pageNumber - 1) * rowsPerPage;
            int totalPages = (filteredTotal + rowsPerPage - 1) / rowsPerPage;

            var boardRows = await boardsQuery
                .Skip(skip)
                .Take(rowsPerPage)
                .ToListAsync();   

            return new()
            {
                Rows = boardRows,
                PageNumber = pageNumber,
                RowsPerPage = rowsPerPage,
                TotalPages = totalPages,
                Summary = new()
                {
                    TotalBoards = filteredTotal
                }
            };      
        }

        public static async Task<PaginatedData<List<TopicBasicInfo>, TopicSummary>> GetTopicsAsync(int pageNumber, int rowsPerPage, string searchTerm = "")
        {
            using var db = new AppDbContext();

            var topicsQuery = from tp in db.Topics
                            //Include the threads with the matching topic id.
                            join th in db.Threads on tp.TopicId equals th.TopicId into threads
                            //Find and include the user
                            join u in db.Users on tp.CreatedByUserId equals u.UserId
                            select new TopicBasicInfo
                            {
                                Topic = tp,
                                TotalThreads = threads.Count(),
                                //Find any threads in the group not older than 5 days since creation
                                NumNewThreads = threads.Count(t => t.CreatedDate > DateTime.UtcNow.AddDays(-5)),
                                TotalPosts = threads.Sum(t => t.Posts.Count),
                                CreatedByUser = new UserBasicInfo()
                                {
                                    User = u
                                }
                            };
            if(!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                topicsQuery = (from t in topicsQuery
                            where t.Topic.TopicName.ToLower().Contains(searchTerm)
                            || t.Topic.Description.ToLower().Contains(searchTerm)
                            || t.Topic.CreatedByUserId.ToLower().Contains(searchTerm)
                            || t.Topic.CreatedDate.ToString().ToLower().Contains(searchTerm)
                            select t);
            }

            var filteredTotal = await topicsQuery.CountAsync();
            
            var skip = (pageNumber - 1) * rowsPerPage;
            int totalPages = (filteredTotal + rowsPerPage - 1) / rowsPerPage;

            var topicRows = await topicsQuery
                .Skip(skip)
                .Take(rowsPerPage)
                .ToListAsync();

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

        public static async Task<PaginatedData<List<PostBasicInfo>, PostSummary>> GetPostsAsync(int pageNumber, int rowsPerPage, string searchTerm = "")
        {
            using var db = new AppDbContext();

            var postsQuery = from p in db.Posts
                            join u in db.Users on p.CreatedByUserId equals u.UserId
                            join th in db.Threads on p.ThreadId equals th.ThreadId
                            join tp in db.Topics on th.TopicId equals tp.TopicId
                            orderby p.CreatedDate descending
                            select new PostBasicInfo
                            {
                                Post = p,
                                CreatedByUser = new UserBasicInfo 
                                {
                                    User = u
                                },
                                Thread = new ThreadBasicInfo
                                {
                                    Thread = th,
                                    CreatedByUser = new UserBasicInfo 
                                    {
                                        User = u
                                    },
                                    Topic = new TopicBasicInfo
                                    {
                                        Topic = tp,
                                        CreatedByUser = new UserBasicInfo 
                                        {
                                            User = u
                                        },                                        
                                    }
                                }
                            };
            if(!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                postsQuery = (from p in postsQuery
                            where p.Post.ReportReason.ToLower().Contains(searchTerm) ||
                            p.Post.PostContent.ToLower().Contains(searchTerm) ||
                            p.CreatedByUser.User.Username.ToLower().Contains(searchTerm) ||
                            p.CreatedByUser.User.UserFirstname.ToLower().Contains(searchTerm) ||
                            p.CreatedByUser.User.UserLastname.ToLower().Contains(searchTerm) ||
                            p.CreatedByUser.UserFullname.ToLower().Contains(searchTerm)
                            select p);
            }

            var filteredTotal = await postsQuery.CountAsync();

            var skip = (pageNumber - 1) * rowsPerPage;
            int totalPages = (filteredTotal + rowsPerPage - 1) / rowsPerPage;

            var postRows = await postsQuery
                .Skip(skip)
                .Take(rowsPerPage)
                .ToListAsync();    

            return new()
            {
                Rows = postRows,
                PageNumber = pageNumber,
                RowsPerPage = rowsPerPage,
                TotalPages = totalPages,
                Summary = new()
                {
                    TotalPosts = filteredTotal
                }
            };      
        }

        public static async Task DeleteCategoryAsync(string categoryId)
        {
            using var db = new AppDbContext();

            var categoryToDelete = await db.Categories.SingleAsync(c => c.CategoryId == categoryId);
            if(categoryToDelete == null)
            {
                throw ClientInducedException.MessageOnly("No such category exists");
            }

            db.Remove(categoryToDelete);
            await db.SaveChangesAsync();
        }
        public static async Task DeleteBoardAsync(string boardId)
        {
            using var db = new AppDbContext();

            var boardToDelete = await db.Boards.SingleAsync(b => b.BoardId == boardId);
            if (boardToDelete == null)
            {
                throw ClientInducedException.MessageOnly("No such board exists.");
            }
            
            db.Remove(boardToDelete);
            await db.SaveChangesAsync();
        }

        public static async Task DeleteTopicAsync(string topicId)          
        {
            using var db = new AppDbContext();

            var topicToDelete = await db.Topics.SingleAsync(t => t.TopicId == topicId);

            if(topicToDelete == null)
            {
                throw ClientInducedException.MessageOnly("No such topic exists.");
            }

            db.Remove(topicToDelete);
            await db.SaveChangesAsync();
        }
    }
}