using Microsoft.AspNetCore.Mvc;
using RestApiServer.Endpoints.ApiResponses;
using RestApiServer.Dto.Admin;
using RestApiServer.Dto.AdminLogin;
using RestApiServer.Endpoints.Services.Admin;
using RestApiServer.Dto.Login;
using RestApiServer.Common.Services;
using Microsoft.AspNetCore.Authorization;
using RestApiServer.Endpoints.Services;
using RestApiServer.Dto.App;

namespace RestApiServer.Endpoints.Controllers.Admin
{
    [ApiController]
    [Route("v1/adminportal")]
    public class AdminPortalUserRegistrationController : ControllerBase
    {
        [HttpPost("user/register")]
        public async Task<ApiSuccessResponse<UserBasicInfo>> RegisterUser(AdminPortalUserRegistrationRequest req)
        {
            var res = await AdminPortalUserRegistrationService.RegisterUser(req);
            return ApiSuccessResponses.WithData("User registration successful", res);
        }
    }
}