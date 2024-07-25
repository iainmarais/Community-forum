using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestApiServer.Core.ApiResponses;
using RestApiServer.Dto.App;
using RestApiServer.Dto.Forum;
using RestApiServer.Services.Categories;
using RestApiServer.Utils;

namespace RestApiServer.Controllers.Categories
{
    [ApiController]
    [Route("v1/forum")]
    public class TopicController : ControllerBase
    {

        [HttpGet("topics/{topicId}/fullinfo")]
        public async Task<ApiSuccessResponse<TopicFullInfo>> GetTopicFullInfo(string topicId)
        {
            var user = AuthUtils.GetForumUserContext(User);
            var res = await TopicService.GetForumTopicFullInfoAsync(topicId);
            return ApiSuccessResponses.WithData("Get topic full info successful", res);
        }
        [HttpGet("topics")]
        public async Task<ApiSuccessResponse<List<TopicBasicInfo>>> GetTopics()
        {
            var res = await TopicService.GetForumTopicsAsync();
            return ApiSuccessResponses.WithData("Get forum topics successful", res);
        }

        [HttpGet("topics/newest")]
        public async Task<ApiSuccessResponse<List<TopicBasicInfo>>> GetNewestTopics()
        {
            var res = await TopicService.GetNewestForumTopicsAsync();
            return ApiSuccessResponses.WithData("Get newest forum topics successful", res);
        }

        [HttpGet("topics/popular")]
        public async Task<ApiSuccessResponse<List<TopicBasicInfo>>> GetPopularTopics()
        {
            var res = await TopicService.GetPopularForumTopicsAsync();
            return ApiSuccessResponses.WithData("Get popular forum topics successful", res);
        }  

        [HttpPost("topics/create")]
        public async Task<ApiSuccessResponse<List<TopicBasicInfo>>> CreateTopic(CreateTopicRequest request)
        {
            var user = AuthUtils.GetForumUserContext(User);
            var res = await TopicService.CreateForumTopicAsync(user.UserId, request);
            return ApiSuccessResponses.WithData("Create topic successful", res);
        }
    }  
}