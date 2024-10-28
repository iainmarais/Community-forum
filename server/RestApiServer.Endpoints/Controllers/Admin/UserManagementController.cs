using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestApiServer.Common.Services;
using RestApiServer.Db;
using RestApiServer.Dto.Admin;
using RestApiServer.Dto.App;
using RestApiServer.Endpoints.ApiResponses;
using RestApiServer.Endpoints.Dto.Admin;
using RestApiServer.Endpoints.Services.Admin;

namespace RestApiServer.Endpoints.Controllers.Admin
{
    [ApiController]
    [Route("v1/adminportal")]
    public class UserManagementController : ControllerBase
    {
        [HttpPost("edituser")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiSuccessResponse<UserBasicInfo>> EditUser(EditUserRequest request)
        {
            var user = AuthService.GetAdminUserContext(User);
            var res = await UserManagementService.EditUserAsync(user.AdminUserId, request);
            return ApiSuccessResponses.WithData("Edit user successful", res);
        }
        [HttpGet("users")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiSuccessResponse<PaginatedData<List<UserFullInfo>, UserSummary>>> GetUsers([FromQuery] int pageNumber, [FromQuery] int rowsPerPage, [FromQuery] string? searchTerm)
        {
            var user = AuthService.GetAdminUserContext(User);
            var res = await UserManagementService.GetUsersAsync(user.AdminUserId, pageNumber, rowsPerPage, searchTerm);
            return ApiSuccessResponses.WithData("Get users successful", res);
        }

        //Endpoint for getting banned users
        [HttpGet("users/banned")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiSuccessResponse<PaginatedData<List<BannedUserBasicInfo>, BannedUserSummary>>> GetBannedUsers([FromQuery] int pageNumber, [FromQuery] int rowsPerPage, [FromQuery] string? searchTerm)
        {
            var user = AuthService.GetAdminUserContext(User);
            var res = await UserManagementService.GetBannedUsersAsync(user.AdminUserId, pageNumber, rowsPerPage, searchTerm);
            return ApiSuccessResponses.WithData("Get banned users successful", res);
        }

        [HttpPost("users/{userId}/ban")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiSuccessResponse<BannedUserBasicInfo>> BanUser(string userId, BanUserRequest request)
        {
            var user = AuthService.GetAdminUserContext(User);
            var res = await UserManagementService.BanUserAsync(user.AdminUserId, userId, request);
            return ApiSuccessResponses.WithData("Ban user successful", res);
        }

        [HttpPost("users/{userId}/delete")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiSuccessResponse<object>> DeleteUser(string userId)
        {
            var user = AuthService.GetAdminUserContext(User);
            await UserManagementService.DeleteUserAsync(user.UserId, userId);
            return ApiSuccessResponses.WithoutData("Delete user successful");
        }

        [HttpGet("users/roles")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiSuccessResponse<List<RoleEntry>>> GetRoles()
        {
            var user = AuthService.GetAdminUserContext(User);
            var res = await UserManagementService.GetRolesAsync();
            return ApiSuccessResponses.WithData("Get available roles: successful", res);
        }

        [HttpPost("users/{userId}/assignrole")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiSuccessResponse<object>> AssignRole(string userId, AssignRoleRequest request)
        {
            var user = AuthService.GetAdminUserContext(User);
            await UserManagementService.AssignRoleAsync(userId, request);
            return ApiSuccessResponses.WithoutData("Assign role successful");
        }

        [HttpPost("users/add")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiSuccessResponse<UserBasicInfo>> AddUser(AddUserRequest request)
        {
            var user = AuthService.GetAdminUserContext(User);
            var res = await UserManagementService.AddUserAsync(request);
            return ApiSuccessResponses.WithData("Add user successful", res);
        }        
    }

}