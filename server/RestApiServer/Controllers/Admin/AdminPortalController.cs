using Microsoft.AspNetCore.Mvc;
using RestApiServer.Core.ApiResponses;
using RestApiServer.Dto.Admin;
using RestApiServer.Services.Admin;
using RestApiServer.Utils;

namespace RestApiServer.Controllers.Admin
{
    [ApiController]
    [Route("v1/adminportal")]
    public class AdminPortalController : ControllerBase
    {
        [HttpGet("appstate")]
        public async Task<ApiSuccessResponse<AdminPortalAppState>> GetAdminPortalAppState()
        {
            var user = AuthUtils.GetAdminUserContext(User);
            var res = await AdminPortalService.GetAdminPortalAppStateAsync(user.UserId);
            return ApiSuccessResponses.WithData("Get admin portal app state successful", res);
        }
    }
}