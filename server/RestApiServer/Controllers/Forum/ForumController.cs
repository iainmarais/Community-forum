using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestApiServer.Common.Services;
using RestApiServer.Core.ApiResponses;
using RestApiServer.Dto.Forum;
using RestApiServer.Services.Forum;

namespace RestApiServer.Controllers.Forum
{
    [ApiController]
    [Route("v1/forum")]
    public class ForumController : ControllerBase
    {
        [HttpGet("state")]
        [Authorize]
        public async Task<ApiSuccessResponse<ForumAppState>> GetAppState()
        {
            var user = AuthService.GetForumUserContext(User);
            var svc = new ForumService();
            var res = await svc.GetForumAppStateAsync(user.UserId);
            return ApiSuccessResponses.WithData("Get forum state successful", res);
        }

        [HttpGet("public/state")]
        public async Task<ApiSuccessResponse<ForumAppState>> GetPublicAppState()
        {
            var svc = new ForumService();
            var res = await svc.GetForumPublicAppStateAsync();
            return ApiSuccessResponses.WithData("Get forum state successful", res);
        }

    }
}