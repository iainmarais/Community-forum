using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestApiServer.Common.Services;
using RestApiServer.Dto.App;
using RestApiServer.Dto.Forum;
using RestApiServer.Endpoints.ApiResponses;
using RestApiServer.Endpoints.Services.Admin;

namespace RestApiServer.Endpoints.Controllers.Admin
{
    [ApiController]
    [Route("v1/adminportal")]
    public class ContentManagementController : ControllerBase
    {
        [HttpPost("categories/{categoryId}/delete")]
        [Authorize(Roles="Admin")]
        public async Task<ApiSuccessResponse<object>> DeleteCategory(string categoryId)
        {
            var user = AuthService.GetAdminUserContext(User);
            await ContentManagementService.DeleteCategoryAsync(categoryId);
            return ApiSuccessResponses.WithoutData("Category deleted successfully");
        }

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
        //Create: topics, categories, boards only
        [HttpPost("categories/create")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiSuccessResponse<object>> CreateCategory(Dto.Admin.AdminCreateCategoryRequest request)
        {
            var user = AuthService.GetAdminUserContext(User);
           await ContentManagementService.CreateCategoryAsync(user.UserId, request);
            return ApiSuccessResponses.WithoutData("Create category successful");
        }

        [HttpPost("boards/create")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiSuccessResponse<object>> CreateBoard(Dto.Admin.AdminCreateBoardRequest request)
        {
            var user = AuthService.GetAdminUserContext(User);
            await ContentManagementService.CreateBoardAsync(user.UserId, request);
            return ApiSuccessResponses.WithoutData("Create board successful");
        }

        [HttpPost("topics/create")]
        [Authorize(Roles = "Admin")]      
        public async Task<ApiSuccessResponse<object>> CreateTopic(Dto.Admin.AdminCreateTopicRequest request)
        {
            var user = AuthService.GetAdminUserContext(User);
            await ContentManagementService.CreateTopicAsync(user.UserId, request);
            return ApiSuccessResponses.WithoutData("Create topic successful");
        }
        //Getters
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

        [HttpGet("posts")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiSuccessResponse<PaginatedData<List<PostBasicInfo>, PostSummary>>> GetPostsAsync([FromQuery] int pageNumber, [FromQuery] int rowsPerPage, [FromQuery] string searchTerm = "")
        {
            var user = AuthService.GetAdminUserContext(User);
            var res = await ContentManagementService.GetPostsAsync(pageNumber, rowsPerPage, searchTerm);
            return ApiSuccessResponses.WithData("Get posts successful", res);            
        }

        [HttpGet("categories/{categoryId}/fullinfo")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResponse<CategoryFullInfo>> GetCategoryFullInfo(string categoryId)
        {
            try
            {
                var user = AuthService.GetAdminUserContext(User);
                var res = await ContentManagementService.GetCategoryFullInfoAsync(categoryId);
                return ApiSuccessResponses.WithData("Get category full info successful", res);
            }
            catch (Exception ex)
            {
                var nullResponse = new CategoryFullInfo
                {
                    Category = new()
                    {
                        CategoryId = "",
                        CategoryName = "",
                        CategoryDescription = "",
                        CreatedByUserId = ""
                    },
                    Boards = new List<BoardBasicInfo>(),
                    TotalBoards = 0
                };
                return ApiClientErrorResponses.WithData(ex.Message, nullResponse);
            }
        }

        [HttpGet("boards/{boardId}/fullinfo")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResponse<BoardFullInfo>> GetBoardFullInfo(string boardId)
        {
            try
            {
                var user = AuthService.GetAdminUserContext(User);
                var res = await ContentManagementService.GetBoardFullInfoAsync(boardId);
                return ApiSuccessResponses.WithData("Get board full info successful", res);
            }
            catch (Exception ex)
            {
                var nullResponse = new BoardFullInfo
                {
                    Board = new(),
                    CreatedByUser = new UserBasicInfo()
                    {
                        User = new()
                    },
                    Topics = new List<TopicBasicInfo>(),
                    TotalTopics = 0
                };
                return ApiClientErrorResponses.WithData(ex.Message, nullResponse);
            }
        }
    }
}