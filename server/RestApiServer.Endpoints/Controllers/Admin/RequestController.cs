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
        /// <summary>
        /// Retrieves a list of support requests that the administrator can see.
        /// </summary>
        /// <param name="searchTerm">The search term to use when filtering the results.</param>
        /// <param name="pageSize">The number of rows to display per page.</param>
        /// <param name="pageNumber">The page number of the data to retrieve.</param>
        /// <returns>The list of support requests.</returns>
        [HttpGet("supportrequests")]
        public async Task<ApiSuccessResponse<PaginatedData<List<RequestBasicInfo>, RequestSummary>>> GetSupportRequestsAsync(
            [FromQuery] string? searchTerm,
            [FromQuery] int pageSize,
            [FromQuery] int pageNumber)
        {
            var adminUser = AuthService.GetAdminUserContext(User);
            var supportRequests = await RequestService.GetSupportRequestsAsync(
                adminUserId: adminUser.AdminUserId,
                pageNumber: pageNumber,
                rowsPerPage: pageSize,
                searchTerm: searchTerm);

            return ApiSuccessResponses.WithData("Get all support requests: successful", supportRequests);
        }

        /// <summary>
        /// Creates a support request.
        /// </summary>
        /// <param name="request">The request containing the support request title and content.</param>
        /// <returns>The newly created support request.</returns>
        [HttpPost("supportrequests/create")]
        public async Task<ApiSuccessResponse<RequestBasicInfo>> CreateSupportRequest([FromBody] CreateAdminPortalRequest request)
        {
            var adminUser = AuthService.GetAdminUserContext(User);
            var supportRequest = await RequestService.CreateSupportRequestAsync(
                adminUserId: adminUser.AdminUserId,
                requestTitle: request.SupportRequestTitle,
                requestContent: request.SupportRequestContent
            );
            return ApiSuccessResponses.WithData("New support request created successfully", supportRequest);
        }

        /// <summary>
        /// Retrieves all support requests for the given user ID. 
        /// Technically, this uses the same service as the other method that gets all support requests. 
        /// This allows for retrieval of all requests created by given user, which might be helpful in auditing.
        /// </summary>
        /// <param name="userId">The user ID of the user for whom to retrieve the support requests.</param>
        /// <param name="searchTerm">The search term to use when filtering the results.</param>
        /// <param name="rowsPerPage">The number of rows to display per page.</param>
        /// <param name="pageNumber">The page number of the data to retrieve.</param>
        /// <returns>The list of support requests, filtered and paged accordingly.</returns>
        [HttpGet("supportrequests/{userId}")]
        public async Task<ApiSuccessResponse<PaginatedData<List<RequestBasicInfo>, RequestSummary>>> GetSupportRequestsByUser(
            string userId, [FromQuery] string? searchTerm, [FromQuery] int rowsPerPage, [FromQuery] int pageNumber)
        {
            var adminUser = AuthService.GetAdminUserContext(User);
            var supportRequests = await RequestService.GetSupportRequestsAsync(
                adminUserId: adminUser.AdminUserId, pageNumber: pageNumber,
                rowsPerPage: rowsPerPage, searchTerm: searchTerm, userId: userId);
            return ApiSuccessResponses.WithData("Get all support requests: successful", supportRequests);
        }

        /// <summary>
        /// Updates the triage status of a support request.
        /// </summary>
        /// <param name="requestId">The ID of the request to update.</param>
        /// <param name="request">The request containing the triage type and triage status to set.</param>
        /// <returns>The updated request.</returns>
        [HttpPost("supportrequests/{requestId}/triage")]
        public async Task<ApiSuccessResponse<RequestBasicInfo>> UpdateRequestTriageStatus(
            string requestId, [FromBody] UpdateRequestTriageStatusRequest request)
        {
            var adminUser = AuthService.GetAdminUserContext(User);
            var updatedRequest = await RequestService.UpdateRequestTriageStatusAsync(
                userId: adminUser.UserId,
                requestId: requestId,
                triageType: request.TriageType,
                triageStatus: request.TriageStatus
            );
            return ApiSuccessResponses.WithData("Request triage status updated successfully", updatedRequest);
        }

        [HttpPost("supportrequests/{requestId}/assign")]
        public async Task<ApiSuccessResponse<RequestBasicInfo>> AssignRequestToUser(string requestId, [FromBody] AssignRequestToUserRequest request)
        {
            var adminUser = AuthService.GetAdminUserContext(User);
            var updatedRequest = await RequestService.AssignRequestToUserAsync(
                adminUserId: adminUser.AdminUserId,
                requestId: requestId,
                assignToUserId: request.UserId
            );
            return ApiSuccessResponses.WithData("Request assigned successfully", updatedRequest);
        }
    }
}