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
    public class RequestController : ControllerBase
    {
        [HttpGet("supportrequests/")]
        public async Task<ApiSuccessResponse<PaginatedData<List<RequestBasicInfo>, RequestSummary>>> GetSupportRequests([FromQuery] string? searchTerm,
                                                                                                                        [FromQuery] int rowsPerPage,
                                                                                                                        [FromQuery] int pageNumber)
        {
            var user = AuthService.GetAdminUserContext(User);
            var res = await RequestService.GetSupportRequestsAsync(user.AdminUserId,
                                                                   pageNumber,
                                                                   rowsPerPage,
                                                                   searchTerm);
            return ApiSuccessResponses.WithData("Get all support requests: successful", res);
        }

        [HttpPost("supportrequests/create")]
        public async Task<ApiSuccessResponse<RequestBasicInfo>> CreateSupportRequest([FromBody]CreateAdminPortalSRRequest req)
        {
            var user = AuthService.GetAdminUserContext(User);
            var res = await RequestService.CreateSupportRequestAsync(adminUserId: user.AdminUserId,
                                                                     supportRequestTitle: req.SupportRequestTitle,
                                                                     supportRequestContent: req.SupportRequestContent);
            return ApiSuccessResponses.WithData("New support request created successfully", res);
        }

        [HttpGet("supportrequests/{userId}")]
        public async Task<ApiSuccessResponse<PaginatedData<List<RequestBasicInfo>, RequestSummary>>> GetSupportRequestsByUser(string userId,
                                                                                                                              [FromQuery] string? searchTerm,
                                                                                                                              [FromQuery] int rowsPerPage,
                                                                                                                              [FromQuery] int pageNumber)
        {
            var user = AuthService.GetAdminUserContext(User);
            var res = await RequestService.GetSupportRequestsAsync(adminUserId: user.AdminUserId, pageNumber: pageNumber,
                                                                   rowsPerPage: rowsPerPage, searchTerm: searchTerm,
                                                                   userId: userId);
            return ApiSuccessResponses.WithData("Get all support requests: successful", res);
        }
    }
}