using Microsoft.AspNetCore.Mvc;
using RestApiServer.Endpoints.ApiResponses;
using RestApiServer.Dto.Admin;
using RestApiServer.Dto.AdminLogin;
using RestApiServer.Endpoints.Services.Admin;
using RestApiServer.Dto.Login;
using RestApiServer.Common.Services;
using Microsoft.AspNetCore.Authorization;
using RestApiServer.Endpoints.Services;

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
    
        [HttpPost("auth/refresh")]
        [Authorize]
        public async Task<ApiSuccessResponse<UserRefreshResponse>> RefreshUserSession(string refreshToken)
        {
            //Todo:
            //When dealing with a bearer token, I need to see what the user context is. 
            //Either that, or I need to create a dedicated admin portal for the forum.
            var user = AuthService.GetAdminUserContext(User);
            var req = new UserRefreshRequest
            {
                RefreshToken = refreshToken,
                UserContext = "admin"
            };
            var res = await UserService.RefreshUserSessionAsync(user.UserId, req);
            return ApiSuccessResponses.WithData("User auth state refresh successful", res);
        }
    }
}