using Microsoft.AspNetCore.Mvc;
using RestApiServer.Endpoints.ApiResponses;
using RestApiServer.Dto.Admin;
using RestApiServer.Dto.AdminLogin;
using RestApiServer.Endpoints.Services.Admin;

namespace RestApiServer.Endpoints.Controllers.Admin
{
    [ApiController]
    [Route("v1/adminportal")]
    public class AdminLoginController : ControllerBase
    {
        [HttpPost("login")]
        public async Task<ApiSuccessResponse<AdminUserLoginResponse>> AdminLogin(AdminUserLoginRequest req)
        {
            //Removed user check since all user validation is being handled by the service code.
            var res = await AdminLoginService.AdminLoginAsync(req);
            return ApiSuccessResponses.WithData("User login successful", res);
        }
    }
}