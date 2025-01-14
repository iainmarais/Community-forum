using Microsoft.AspNetCore.Mvc;
using RestApiServer.Endpoints.ApiResponses;
using RestApiServer.Dto.Admin;
using RestApiServer.Dto.AdminLogin;
using RestApiServer.Endpoints.Services.Admin;
using RestApiServer.Dto.Login;
using RestApiServer.Common.Services;
using Microsoft.AspNetCore.Authorization;
using RestApiServer.Endpoints.Services;
using RestApiServer.Db;

namespace RestApiServer.Endpoints.Controllers.Admin
{
    [ApiController]
    [Route("v1/adminportal")]
    public class SupportRequestController : ControllerBase
    {
        [HttpGet("supportrequests/{userId}")]
        public async Task<ApiSuccessResponse<PaginatedData<List<SupportRequestBasicInfo>, SupportRequestSummary>>> GetSupportRequests([FromQuery]string? searchTerm, [FromQuery]int rowsPerPage, [FromQuery]int pageNumber)
        {
            var user = AuthService.GetAdminUserContext(User);
            var res = await SupportRequestService.GetSupportRequestsAsync(user.AdminUserId, pageNumber, rowsPerPage, searchTerm);
            return ApiSuccessResponses.WithData("Get all support requests: successful", res);
        }
    }
}