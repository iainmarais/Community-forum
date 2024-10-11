using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestApiServer.Core.ApiResponses;
using RestApiServer.Dto.Admin;
using RestApiServer.Dto.AdminLogin;
using RestApiServer.Services.Admin;
using RestApiServer.Utils;

namespace RestApiServer.Controllers.Admin
{
    [ApiController]
    [Route("v1/admin")]
    public class AdminLoginController : ControllerBase
    {
        [HttpPost("login")]
        //Only administrators should have login access to this endpoint.
        [Authorize(Roles = "admin")]
        public async Task<ApiSuccessResponse<AdminUserLoginResponse>> AdminLogin(AdminUserLoginRequest req)
        {
            var user = AuthUtils.GetAdminUserContext(User);
            var res = await AdminLoginService.AdminLoginAsync(req);
            return ApiSuccessResponses.WithData("User login successful", res);
        }
    }
}