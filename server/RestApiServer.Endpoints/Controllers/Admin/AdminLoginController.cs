using Microsoft.AspNetCore.Mvc;
using RestApiServer.Endpoints.ApiResponses;
using RestApiServer.Dto.Admin;
using RestApiServer.Dto.AdminLogin;
using RestApiServer.Endpoints.Services.Admin;
using RestApiServer.Dto.Login;
using RestApiServer.Common.Services;
using RestApiServer.Endpoints.Services;
using RestApiServer.Dto.App;
using Serilog;

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
        public async Task<ApiResponse<UserRefreshResponse>> RefreshUserSession([FromBody] RefreshTokenRequest req)
        {
            try
            {
                //The issue that happens here is that this fails due to the expired JWT. 
                //The failure is expected and used by all other endpoints to trigger a logoff, but here it should not fail but rather grab the userid claim
                var refreshRequest = new UserRefreshRequest
                {
                    RefreshToken = req.RefreshToken,
                    UserContext = "admin"
                };
                var res = await UserService.RefreshUserSessionAsync(req.LoggedInUserId, refreshRequest);
                return ApiSuccessResponses.WithData("User auth state refresh successful", res);
            }
            catch (Exception ex)
            {
                //Create a functionally null response to send back. This satisfies the need for a non-null payload. The error tells the client there's a fault and it will log off accordingly.
                var nullResponse = new UserRefreshResponse
                {
                    NewAccessToken = "",
                    NewAccessTokenExpiration = 0,
                    RefreshToken = "",
                    UserProfile = new UserBasicInfo()
                    {
                        User=new()
                    }
                };
                Log.Error(ex.Message);
                return ApiClientErrorResponses.WithData(ex.Message, nullResponse);
            }
        }
        //Used here for now. Will be moved to its own DTO file.
        public class RefreshTokenRequest
        {
            public required string RefreshToken { get; set; }
            public required string LoggedInUserId { get; set; }
        }
    }
}