using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestApiServer.Common.Services;
using RestApiServer.Endpoints.ApiResponses;
using RestApiServer.Dto.Admin;
using RestApiServer.Dto.App;
using RestApiServer.Endpoints.Services.Admin;
using RestApiServer.Endpoints.Dto.Admin;

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
    }
}