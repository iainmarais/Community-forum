using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestApiServer.Common.Services;
using RestApiServer.Dto.Forum;
using RestApiServer.Endpoints.ApiResponses;
using RestApiServer.Endpoints.Services.Admin;

namespace RestApiServer.Endpoints.Controllers.Admin
{
    [ApiController]
    [Route("v1/adminportal")]
    public class ContentManagementController : ControllerBase
    {
        [HttpPost("boards/{boardId}/delete")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiSuccessResponse<object>> DeleteBoard(string boardId)
        {
            var user = AuthService.GetAdminUserContext(User);
            await ContentManagementService.DeleteBoardAsync(boardId);
            return ApiSuccessResponses.WithoutData("Board deleted successfully.");
        }

        [HttpPost("topics/{topicId}/delete")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiSuccessResponse<object>> DeleteTopic(string topicId)
        {
            var user = AuthService.GetAdminUserContext(User);
            await ContentManagementService.DeleteTopicAsync(topicId);
            return ApiSuccessResponses.WithoutData("Topic deleted successfully.");
        }


        [HttpPost("boards/create")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiSuccessResponse<object>> CreateBoard(Dto.Admin.CreateBoardRequest request)
        {
            var user = AuthService.GetAdminUserContext(User);
            await ContentManagementService.CreateBoardAsync(user.UserId, request);
            return ApiSuccessResponses.WithoutData("Create board successful");
        }

        [HttpPost("categories/create")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiSuccessResponse<object>> CreateCategory(Dto.Admin.CreateCategoryRequest request)
        {
            var user = AuthService.GetAdminUserContext(User);
           await ContentManagementService.CreateCategoryAsync(user.UserId, request);
            return ApiSuccessResponses.WithoutData("Create category successful");
        }

        [HttpPost("topics/create")]
        [Authorize(Roles = "Admin")]      
        public async Task<ApiSuccessResponse<object>> CreateTopic(Dto.Admin.CreateTopicRequest request)
        {
            var user = AuthService.GetAdminUserContext(User);
            await ContentManagementService.CreateTopicAsync(user.UserId, request);
            return ApiSuccessResponses.WithoutData("Create topic successful");
        }

        [HttpGet("categories")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiSuccessResponse<PaginatedData<List<CategoryBasicInfo>, CategorySummary>>> GetCategoriesAsync([FromQuery] int pageNumber, [FromQuery] int rowsPerPage, [FromQuery] string searchTerm = "")
        {
            var user = AuthService.GetAdminUserContext(User);
            var res = await ContentManagementService.GetCategoriesAsync(pageNumber, rowsPerPage, searchTerm);
            return ApiSuccessResponses.WithData("Get categories successful", res);
        }

        [HttpGet("boards")]
        [Authorize(Roles = "Admin")]  
        public async Task<ApiSuccessResponse<PaginatedData<List<BoardBasicInfo>,BoardSummary>>> GetBoardsAsync([FromQuery] int pageNumber, [FromQuery] int rowsPerPage, [FromQuery] string searchTerm = "")
        {
            var user = AuthService.GetAdminUserContext(User);
            var res = await ContentManagementService.GetBoardsAsync(pageNumber, rowsPerPage, searchTerm);
            return ApiSuccessResponses.WithData("Get boards successful", res);
        }
        
        [HttpGet("topics")]
        [Authorize(Roles = "Admin")]    
        public async Task<ApiSuccessResponse<PaginatedData<List<TopicBasicInfo>, TopicSummary>>> GetTopicsAsync([FromQuery] int pageNumber, [FromQuery] int rowsPerPage, [FromQuery] string searchTerm = "")
        {
            var user = AuthService.GetAdminUserContext(User);
            var res = await ContentManagementService.GetTopicsAsync(pageNumber, rowsPerPage, searchTerm);
            return ApiSuccessResponses.WithData("Get topics successful", res);
        }  
    }
}