using RestApiServer.Core.ApiResponses;
using RestApiServer.Services;
using Microsoft.AspNetCore.Mvc;
using RestApiServer.Db.Users;
using RestApiServer.Dto.Login;
using RestApiServer.Dto.App;
using RestApiServer.Dto.Forum;
using RestApiServer.Utils;
using Microsoft.AspNetCore.Authorization;
using RestApiServer.Core.Security;
using RestApiServer.Security;

namespace RestApiServer.Controllers
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
        public async Task<ApiSuccessResponse<UserRefreshResponse>> RefreshUserSession(UserRefreshRequest req)
        {
            //Todo: Change this user context getter to listen for the user context and return the appropriate one, and update all endpoints where it is used.
            var user = AuthUtils.GetForumUserContext(User);
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
            var user = AuthUtils.GetAdminUserContext(User);
            var res = await UserService.UpdateUserProfileAsync(request);
            return ApiSuccessResponses.WithData("Update user profile successful", res);
        }

    }
}