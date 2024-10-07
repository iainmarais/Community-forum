
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestApiServer.Core.ApiResponses;
using RestApiServer.Core.Security;
using RestApiServer.Dto.App;
using RestApiServer.Dto.Forum;
using RestApiServer.Services.Discussions;
using RestApiServer.Utils;

namespace RestApiServer.Controllers.Discussions
{
    [ApiController]
    [Route("v1/forum")]
    [Authorize]
    public class DiscussionController : ControllerBase
    {
        [HttpGet("thread/{threadId}/fullinfo")]
        public async Task<ApiSuccessResponse<ThreadFullInfo>> GetThreadFullInfo(string threadId)
        {
            var user = AuthUtils.GetForumUserContext(User);
            var res = await DiscussionService.GetForumThreadFullInfoAsync(threadId);
            return ApiSuccessResponses.WithData("Get discussion state successful", res);
        }
        [HttpPost("threads/create")]
        [Authorize(policy:"CreateThreadsPolicy")]
        public async Task<ApiSuccessResponse<object>> CreateThread(CreateThreadRequest request)
        {        
            var user = AuthUtils.GetForumUserContext(User);
            await DiscussionService.CreateThreadAsync(request.TopicId, request.ThreadName, user.UserId);
            return ApiSuccessResponses.WithoutData("Create forum thread successful");
        }

        [HttpPost("threads/createAndPost")]
        [Authorize(policy:"CreateThreadsPolicy")]
        public async Task<ApiSuccessResponse<object>> CreateThreadWithPost(CreateThreadWithPostRequest request)
        {	
            var user = AuthUtils.GetForumUserContext(User);
            await DiscussionService.CreateThreadWithPostAsync(request.TopicId, request.ThreadName, user.UserId, request.MessageContent);
            return ApiSuccessResponses.WithoutData("Create forum thread successful");
        }  
        [HttpGet("threads")]
        public async Task<ApiSuccessResponse<List<ThreadBasicInfo>>> GetThreadSummary()
        {
            var res = await DiscussionService.GetForumThreadSummaryAsync();
            return ApiSuccessResponses.WithData("Get forum threads successful", res);
        }

        [HttpGet("threads/{topicId}")]
        public async Task<ApiSuccessResponse<List<ThreadBasicInfo>>> GetThreadsByTopic(string topicId)
        {
            var res = await DiscussionService.GetForumThreadsByTopicAsync(topicId);
            return ApiSuccessResponses.WithData("Get forum threads by topic successful", res);
        }

        [HttpGet("thread/{threadId}/posts")]
        public async Task<ApiSuccessResponse<PaginatedData<List<PostFullInfo>, PostSummary>>> GetPaginatedPostsForThread(string threadId, [FromQuery] int pageNumber, [FromQuery] int rowsPerPage, [FromQuery] string? searchTerm)
        {
            var res = await DiscussionService.GetPaginatedPostsForThreadAsync(threadId, pageNumber, rowsPerPage, searchTerm);
            return ApiSuccessResponses.WithData("Get forum thread posts successful", res);
        }
        [HttpPost("thread/{threadId}/posts/create")]
        [Authorize(policy:"CreatePostsPolicy")]
        public async Task<ApiSuccessResponse<PostFullInfo>> CreateThreadPost(CreatePostRequest request)
        {
            var user = AuthUtils.GetForumUserContext(User);
            var res = await DiscussionService.CreatePostAsync(request);
            return ApiSuccessResponses.WithData("Get forum thread posts successful", res);
        }

        [HttpPost("thread/{threadId}/posts/{postId}/report")]
        public async Task<ApiSuccessResponse<string>> ReportPost(ReportPostRequest request)
        {
            var user = AuthUtils.GetForumUserContext(User);
            var res = await DiscussionService.ReportPostAsync(request);
            return ApiSuccessResponses.WithData("Report post successful", res);
        }
    }
}