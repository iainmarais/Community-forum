using Microsoft.EntityFrameworkCore;
using RestApiServer.Db;
using RestApiServer.Dto.App;
using RestApiServer.Db.Users;
using RestApiServer.Utils;
using RestApiServer.Dto.Forum;
using RestApiServer.Core.ApiResponses;

namespace RestApiServer.Services.Discussions
{

    public class DiscussionService
    {
        public static async Task<ThreadFullInfo> GetForumThreadFullInfoAsync(string threadId)
        {
            using var db = new AppDbContext();
            var thread = await db.Threads
            .Include(t => t.Posts.OrderByDescending(m => m.CreatedDate))
            .Include(t => t.CreatedByUser)
            .SingleOrDefaultAsync(t => t.ThreadId == threadId);
            if(thread == null)
            {
                throw new Exception("Thread not found");
            }
            var threadFullInfo = new ThreadFullInfo()
            {
                Thread = thread,
                TotalPosts = thread.Posts.Count,
                Posts = thread.Posts.Select(p => new PostBasicInfo()
                {
                    Post = p
                }).ToList(), 
                CreatedByUser = new UserBasicInfo(thread.CreatedByUser ?? UserEntry.CreateDefaultGuestUser())
            };
            //Need to construct a ThreadFullInfo instance from the thread found by id, the associated messages and the creator.
            //All of these are different tables in the database.
            
            return threadFullInfo ?? throw new Exception("Thread not found");
        }
        public static async Task<ThreadBasicInfo> CreateThreadAsync(string topicId, string threadName, string createdByUserId)
        {
            using var db = new AppDbContext();
            var thread = new ThreadEntry
            {
                ThreadId = DbUtils.GenerateUuid(),
                TopicId = topicId,
                ThreadName = threadName,
                CreatedDate = DateTime.Now,
                CreatedByUserId = createdByUserId,
            };
            await db.Threads.AddAsync(thread);
            await db.SaveChangesAsync();
            return new ThreadBasicInfo()
            {
                Thread = thread,
                TotalPosts = thread.Posts.Count(),
                CreatedByUser = new UserBasicInfo(thread.CreatedByUser ?? UserEntry.CreateDefaultGuestUser())
            };
        }

        
        public static async Task<ThreadBasicInfo> CreateThreadWithPostAsync(string topicId, string threadName, string createdByUserId, string postContent)
        {
            var thread = new ThreadEntry
            {
                ThreadId = DbUtils.GenerateUuid(),
                TopicId = topicId,
                ThreadName = threadName,
                CreatedDate = DateTime.Now,
                CreatedByUserId = createdByUserId
            };

            var message = new PostEntry
            {
                PostId = DbUtils.GenerateUuid(),
                ThreadId = thread.ThreadId,
                PostContent = postContent,
                CreatedDate = DateTime.Now,
                CreatedByUserId = createdByUserId,
                IsFirstPost = true
            };
            using(var db = new AppDbContext())
            {
                using var txn = await db.Database.BeginTransactionAsync();
                try
                {
                    await db.Threads.AddAsync(thread);
                    await db.Posts.AddAsync(message);
                    await db.SaveChangesAsync();
                    await txn.CommitAsync();
                }
                catch (Exception ex)
                {
                    await txn.RollbackAsync();
                    throw new Exception("Could not create post.", ex);
                }
            }
            return new ThreadBasicInfo()
            {
                Thread = thread,
                TotalPosts = thread.Posts.Count(),
                CreatedByUser = new UserBasicInfo(thread.CreatedByUser ?? UserEntry.CreateDefaultGuestUser())
            };
        } 

        public static async Task<List<ThreadBasicInfo>> GetForumThreadSummaryAsync()
        {
            using var db = new AppDbContext();
            //Unless I need to do any alteration or joining here, I am simply going to return the data as a list asynchronously.
            var res = await db.Threads
                                .OrderByDescending(t => t.CreatedDate)
                                .Select(t => new ThreadBasicInfo()
                                {
                                    Thread = t,
                                    TotalPosts = t.Posts.Count(),
                                    CreatedByUser = new UserBasicInfo(t.CreatedByUser ?? UserEntry.CreateDefaultGuestUser())                                    
                                })
                                .ToListAsync();
            return res;
        }

        public static async Task<List<ThreadBasicInfo>> GetForumThreadsByTopicAsync(string topicId)
        {
            using var db = new AppDbContext();
            var res = await (from thread in db.Threads 
                            where thread.TopicId == topicId 
                            join user in db.Users on thread.CreatedByUserId equals user.UserId
                            select new ThreadBasicInfo() 
                            { 
                                Thread = thread,
                                CreatedByUser = new UserBasicInfo(user),
                                TotalPosts = thread.Posts.Count()
                            }).ToListAsync();
            return res;
        }

        public static async Task<PaginatedData<List<PostFullInfo>, PostSummary>> GetPaginatedPostsForThreadAsync(string threadId, int pageNumber, int rowsPerPage, string? searchTerm)
        {
            using var db = new AppDbContext();
            var posts = (from post in db.Posts
                                where post.ThreadId == threadId
                                join user in db.Users on post.CreatedByUserId equals user.UserId
                                select new PostFullInfo()
                                {
                                    Post = post,
                                    CreatedByUser = new UserBasicInfo(user)
                                }).AsEnumerable();
            var filteredPosts = posts;
            if(!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                filteredPosts = (from p in filteredPosts
                                where p.Post.PostContent.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase)
                                || p.CreatedByUser.UserFirstname.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase)
                                || p.CreatedByUser.UserLastname.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase)
                                || p.CreatedByUser.Username.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase)
                                  select p);
            }
            var filteredTotal = filteredPosts.Count();
            var skip = (pageNumber - 1) * rowsPerPage;
            var pageRows = filteredPosts.Skip(skip).Take(rowsPerPage);
            var postRows = new List<PostFullInfo>();
            foreach (var post in pageRows)
            {
                postRows.Add(post);
            }
            int totalPages = 1;
            if(filteredTotal > rowsPerPage)
            {
                totalPages = filteredTotal / rowsPerPage;
            }
            return new() 
            {
                Rows = postRows,
                TotalPages = totalPages,
                PageNumber = pageNumber,
                RowsPerPage = rowsPerPage,
                Summary = new()
                {
                    TotalPosts = filteredTotal
                }
            };                 
        }

        public static async Task<PostFullInfo> CreatePostAsync(CreatePostRequest request)
        {
            using var db = new AppDbContext();
            var post = new PostEntry
            {
                PostId = DbUtils.GenerateUuid(),
                ThreadId = request.ThreadId,
                PostContent = request.PostContent,
                CreatedDate = DateTime.Now,
                CreatedByUserId = request.CreatedByUserId,
                ReplyToPostId = request.ReplyToPostId
            };
            db.Posts.Add(post);
            await db.SaveChangesAsync();
            var createdByUser = await db.Users.Where(u => u.UserId == post.CreatedByUserId).FirstOrDefaultAsync();
            return new PostFullInfo 
            {
                Post = new PostEntry()
                {
                    PostId = post.PostId,
                    ThreadId = post.ThreadId,
                    PostContent = post.PostContent,
                    CreatedDate = post.CreatedDate,
                    CreatedByUserId = post.CreatedByUserId,
                    ReplyToPostId = post.ReplyToPostId
                },
                CreatedByUser = new UserBasicInfo(createdByUser ?? UserEntry.CreateDefaultGuestUser())
            };
        }
        public static async Task<string> ReportPostAsync(ReportPostRequest request)
        {
            using var db = new AppDbContext();
            var post = await db.Posts
                                .Where(m => m.PostId == request.PostId)
                                .FirstOrDefaultAsync();
            if(post == null)
            {
                throw new Exception("Post not found");
            }
            post.PostReported = true;
            post.ReportedByUserId = request.ReportedByUserId;
            post.ReportReason = request.ReportReason;
            await db.SaveChangesAsync();
            var createdByUser = await db.Users.Where(u => u.UserId == post.CreatedByUserId).FirstOrDefaultAsync();
            return $"Reported post by {createdByUser?.Username ?? "Guest"} on {post.CreatedDate} for reason: {post.ReportReason}";
        }           
    }
}