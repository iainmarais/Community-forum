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
        [Authorize]
        public async Task<ApiSuccessResponse<UserRefreshResponse>> RefreshUserSession(UserRefreshRequest req)
        {
            //Todo:
            //When dealing with a bearer token, I need to see what the user context is. 
            //Either that, or I need to create a dedicated admin portal for the forum.
            var user = AuthService.GetForumUserContext(User);
            var res = await UserService.RefreshUserSessionAsync(user.UserId, req);
            return ApiSuccessResponses.WithData("User auth state refresh successful", res);
        }

        [HttpPost("login")]
        public async Task<ApiSuccessResponse<UserLoginResponse>> LoginUser(UserLoginRequest request)
        {
            var res = await UserService.Login(request);
            Console.WriteLine(res.UserProfile);
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

    }
}