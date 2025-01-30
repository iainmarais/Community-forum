using RestApiServer.Endpoints.ApiResponses;
using Microsoft.AspNetCore.Mvc;
using RestApiServer.Dto.Login;
using RestApiServer.Dto.App;
using RestApiServer.Dto.Forum;
using Microsoft.AspNetCore.Authorization;
using RestApiServer.Common.Services;
using RestApiServer.Endpoints.Services;

namespace RestApiServer.Endpoints.Controllers
{
    [ApiController]
    [Route("v1/users")]
    public class UserController : ControllerBase
    {
        //Pass the incoming request through to the service.
        [HttpPost("register")]
        public async Task<ApiSuccessResponse<UserBasicInfo>> RegisterUser(UserRegistrationRequest request)
        {
            var res = await UserService.Register(request);
            return ApiSuccessResponses.WithData("User registration successful", res);

        }
        //This may need some work still, so will not yet use it. If users want to use the chat and get logged off, they can log in again for now.
        [HttpPost("auth/refresh")]
        public async Task<ApiResponse<UserRefreshResponse>> RefreshUserSession([FromBody] RefreshTokenRequest req)
        {
            //Read the UserId from the expired JWT, but how does one instruct it to disregard the expiration
            try
            {
                var refreshRequest = new UserRefreshRequest 
                {
                    RefreshToken = req.RefreshToken,
                    UserContext = "forum"
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
                        User = new()
                    }
                };
                return ApiClientErrorResponses.WithData(ex.Message, nullResponse);
            }
        }

        [HttpPost("login")]
        public async Task<ApiSuccessResponse<UserLoginResponse>> LoginUser(UserLoginRequest request)
        {
            var res = await UserService.Login(request);
            return ApiSuccessResponses.WithData("User login successful", res);
        }

        [HttpGet("{userId}")]
        public async Task<ApiSuccessResponse<UserBasicInfo>> GetUserBasicInfo(string userId)
        {
            var res = await UserService.GetUserBasicInfoAsync(userId);
            return ApiSuccessResponses.WithData("Get user basic info successful", res);
        }

        [HttpPost("{userId}/profile")]
        [Authorize(policy: "EditUsersPolicy")]
        public async Task<ApiSuccessResponse<UserBasicInfo>> UpdateUserProfile(UpdateUserProfileRequest request)
        {
            var user = AuthService.GetAdminUserContext(User);
            var res = await UserService.UpdateUserProfileAsync(request);
            return ApiSuccessResponses.WithData("Update user profile successful", res);
        }

        public class RefreshTokenRequest
        {
            public required string RefreshToken { get; set; }
            public required string LoggedInUserId { get; set; }
        }
    }
}