using Microsoft.AspNetCore.Mvc;
using RestApiServer.Core.ApiResponses;
using RestApiServer.Dto.Admin;
using RestApiServer.Dto.App;
using RestApiServer.Services.Admin;
using RestApiServer.Utils;

namespace RestApiServer.Controllers.Admin
{
    [ApiController]
    [Route("v1/forum/admin")]
    public class AdminController : ControllerBase
    {
        [HttpPost("users/assignrole")]
        public async Task<ApiSuccessResponse<object>> AssignUserRole(AssignRoleRequest request)
        {
            var user = AuthUtils.GetForumUserContext(User);
            await AdminService.AssignUserRoleAsync(user.UserId, request);
            return ApiSuccessResponses.WithoutData("Assign role successful");
        }

        [HttpGet("roles")]
        public async Task<ApiSuccessResponse<List<RoleBasicInfo>>> GetRoles()
        {
            var user = AuthUtils.GetForumUserContext(User);
            var roles = await AdminService.GetRolesAsync(user.UserId);
            return ApiSuccessResponses.WithData("Get roles successful", roles);
        }

        [HttpGet("users")]
        public async Task<ApiSuccessResponse<List<UserBasicInfo>>> GetUsers()
        {
            var user = AuthUtils.GetForumUserContext(User);
            var users = await AdminService.GetUsersAsync(user.UserId);
            return ApiSuccessResponses.WithData("Get users successful", users);
        }
    }
}