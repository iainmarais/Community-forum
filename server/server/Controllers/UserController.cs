using RestApiServer.Core.ApiResponses;
using RestApiServer.Services;
using Microsoft.AspNetCore.Mvc;
using RestApiServer.Db.Users;
using RestApiServer.Dto.Login;
using RestApiServer.Dto.App;

namespace RestApiServer.Controllers
{
    [ApiController]
    [Route("v1/users")]
    public class UserController : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ApiSuccessResponse<UserBasicInfo>> RegisterUser(UserRegistrationRequest request)
        {
            var res = await UserService.Register(request.Username, request.EmailAddress, request.Password, request.RetypePassword);
            return ApiSuccessResponses.WithData("User registration successful", res);
        }

        [HttpPost("login")]
        public async Task<ApiSuccessResponse<UserLoginResponse>> LoginUser(UserLoginRequest request)
        {
            var res = await UserService.Login(request.UserIdentifier, request.Password, "Forum");
            Console.WriteLine(res.UserProfile);
            return ApiSuccessResponses.WithData("User login successful", res);
        }

        [HttpGet("{userId}")]
        public async Task<ApiSuccessResponse<UserBasicInfo>> GetUserBasicInfo(string userId)
        {
            var res = await UserService.GetUserBasicInfoAsync(userId);
            return ApiSuccessResponses.WithData("Get user basic info successful", res);
        }

    }
}