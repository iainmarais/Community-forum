using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestApiServer.Core.ApiResponses;
using RestApiServer.Core.Security;
using RestApiServer.Dto.App;
using RestApiServer.Dto.Forum;
using RestApiServer.Services;
using RestApiServer.Utils;

namespace RestApiServer.Controllers
{
    [ApiController]
    [Route("v1/forum")]
    public class ForumController : ControllerBase
    {
        [HttpGet("state")]
        [Authorize]
        public async Task<ApiSuccessResponse<ForumAppState>> GetAppState()
        {
            var user = AuthUtils.GetForumUserContext(User);
            var res = await ForumService.GetForumAppStateAsync(user.UserId);
            return ApiSuccessResponses.WithData("Get forum state successful", res);
        }

        [HttpGet("public/state")]
        public async Task<ApiSuccessResponse<ForumAppState>> GetPublicAppState()
        {
            var res = await ForumService.GetForumPublicAppStateAsync();
            return ApiSuccessResponses.WithData("Get forum state successful", res);
        }

    }
}