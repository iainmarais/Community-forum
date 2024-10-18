using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestApiServer.Common.Services;
using RestApiServer.Endpoints.ApiResponses;
using RestApiServer.Dto.Admin;
using RestApiServer.Dto.App;
using RestApiServer.Endpoints.Services.Admin;

namespace RestApiServer.Endpoints.Controllers.Admin
{
    [ApiController]
    [Route("v1/forum/admin")]
    public class AdminController : ControllerBase
    {
        [HttpPost("users/assignrole")]
        public async Task<ApiSuccessResponse<object>> AssignUserRole(AssignRoleRequest request)
        {
            var user = AuthService.GetForumUserContext(User);
            await AdminService.AssignUserRoleAsync(user.UserId, request);
            return ApiSuccessResponses.WithoutData("Assign role successful");
        }

        [HttpPost("users/create")]
        public async Task<ApiSuccessResponse<object>> CreateUser(CreateUserRequest req)
        {
            var user = AuthService.GetForumUserContext(User);
            await AdminService.CreateUserAsync(user.UserId, req);
            return ApiSuccessResponses.WithoutData("Create user successful");
        }

        [HttpPost("users/update")]
        public async Task<ApiSuccessResponse<object>> CreateUser(UserBasicInfo userToUpdate)
        {
            var user = AuthService.GetForumUserContext(User);
            await AdminService.UpdateUserAsync(user.UserId, userToUpdate);
            return ApiSuccessResponses.WithoutData("Create user successful");
        }

        [HttpGet("roles")]
        public async Task<ApiSuccessResponse<List<RoleBasicInfo>>> GetRoles()
        {
            var user = AuthService.GetForumUserContext(User);
            var roles = await AdminService.GetRolesAsync(user.UserId);
            return ApiSuccessResponses.WithData("Get roles successful", roles);
        }

        [HttpGet("users")]
        public async Task<ApiSuccessResponse<List<UserBasicInfo>>> GetUsers()
        {
            var user = AuthService.GetForumUserContext(User);
            var users = await AdminService.GetUsersAsync(user.UserId);
            return ApiSuccessResponses.WithData("Get users successful", users);
        }
        [HttpGet("appstate")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiSuccessResponse<AdminPortalAppState>> GetAdminPortalAppState()
        {
            var user = AuthService.GetAdminUserContext(User);
            var appState = await AdminPortalService.GetAdminPortalAppStateAsync(user.UserId);
            return ApiSuccessResponses.WithData("Get app state successful", appState);
        }
    }
}