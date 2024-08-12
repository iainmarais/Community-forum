using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RestApiServer.Core.ApiResponses;
using RestApiServer.Core.Security;
using RestApiServer.Dto.App;
using RestApiServer.Dto.Forum;
using RestApiServer.Hubs;
using RestApiServer.Services;
using RestApiServer.Utils;

namespace RestApiServer.Controllers
{
    [ApiController]
    [Route("v1/forum")]
    public class ForumController : ControllerBase
    {
        private readonly IHubContext<ForumServiceHub> _ForumServiceHub;
        public ForumController(IHubContext<ForumServiceHub> forumServiceHub)
        {
            _ForumServiceHub = forumServiceHub;
        }
        [HttpGet("state")]
        [Authorize]
        public async Task<ApiSuccessResponse<ForumAppState>> GetAppState()
        {
            var user = AuthUtils.GetForumUserContext(User);
            var svc = new ForumService();
            var res = await svc.GetForumAppStateAsync(user.UserId);

            //Send the full app state to the client using SignalR.
            await _ForumServiceHub.Clients.Group(user.UserId).SendAsync("GetForumState", user.UserId, res);

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