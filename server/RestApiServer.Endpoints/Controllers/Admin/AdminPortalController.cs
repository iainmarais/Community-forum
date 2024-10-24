using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestApiServer.Common.Services;
using RestApiServer.Endpoints.ApiResponses;
using RestApiServer.Dto.Admin;
using RestApiServer.Dto.App;
using RestApiServer.Endpoints.Services.Admin;
using RestApiServer.Endpoints.Dto.Admin;
using RestApiServer.Db;

namespace RestApiServer.Endpoints.Controllers.Admin
{
    [ApiController]
    [Route("v1/adminportal")]
    public class AdminPortalController : ControllerBase
    {
        [HttpGet("appstate")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiSuccessResponse<AdminPortalAppState>> GetAdminPortalAppState()
        {
            var user = AuthService.GetAdminUserContext(User);
            var res = await AdminPortalService.GetAdminPortalAppStateAsync(user.UserId);
            return ApiSuccessResponses.WithData("Get admin portal app state successful", res);
        }

        [HttpPost("edituser")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiSuccessResponse<UserBasicInfo>> EditUser(EditUserRequest request)
        {
            var user = AuthService.GetAdminUserContext(User);
            var res = await AdminPortalService.EditUserAsync(user.AdminUserId, request);
            return ApiSuccessResponses.WithData("Edit user successful", res);
        }
        [HttpGet("users")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiSuccessResponse<PaginatedData<List<UserFullInfo>, UserSummary>>> GetUsers([FromQuery] int pageNumber, [FromQuery] int rowsPerPage, [FromQuery] string? searchTerm)
        {
            var user = AuthService.GetAdminUserContext(User);
            var res = await AdminPortalService.GetUsersAsync(user.AdminUserId, pageNumber, rowsPerPage, searchTerm);
            return ApiSuccessResponses.WithData("Get users successful", res);
        }

        //Endpoint for getting banned users
        [HttpGet("users/banned")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiSuccessResponse<PaginatedData<List<BannedUserBasicInfo>, BannedUserSummary>>> GetBannedUsers([FromQuery] int pageNumber, [FromQuery] int rowsPerPage, [FromQuery] string? searchTerm)
        {
            var user = AuthService.GetAdminUserContext(User);
            var res = await AdminPortalService.GetBannedUsersAsync(user.AdminUserId, pageNumber, rowsPerPage, searchTerm);
            return ApiSuccessResponses.WithData("Get banned users successful", res);
        }

        [HttpPost("users/{userId}/ban")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiSuccessResponse<BannedUserBasicInfo>> BanUser(string userId, BanUserRequest request)
        {
            var user = AuthService.GetAdminUserContext(User);
            var res = await AdminPortalService.BanUserAsync(user.AdminUserId, userId, request);
            return ApiSuccessResponses.WithData("Ban user successful", res);
        }

        [HttpPost("users/{userId}/delete")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiSuccessResponse<object>> DeleteUser(string userId)
        {
            var user = AuthService.GetAdminUserContext(User);
            await AdminPortalService.DeleteUserAsync(user.UserId, userId);
            return ApiSuccessResponses.WithoutData("Delete user successful");
        }

        [HttpGet("users/roles")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiSuccessResponse<List<RoleEntry>>> GetRoles()
        {
            var user = AuthService.GetAdminUserContext(User);
            var res = await AdminPortalService.GetRolesAsync();
            return ApiSuccessResponses.WithData("Get available roles: successful", res);
        }
    }
}