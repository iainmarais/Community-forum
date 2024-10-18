using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestApiServer.Common.Services;
using RestApiServer.Endpoints.ApiResponses;
using RestApiServer.Dto.Forum;
using RestApiServer.Endpoints.Services.Forum.Categories;

namespace RestApiServer.Endpoints.Controllers.Forum.Categories
{

    [ApiController]
    [Route("v1/forum")]
    [Authorize]
    public class CategoryController : ControllerBase
    {

        [HttpGet("categories/{categoryId}/fullinfo")]
        public async Task<ApiSuccessResponse<CategoryFullInfo>> GetCategoryFullInfo(string categoryId)
        {
            var user = AuthService.GetForumUserContext(User);
            var res = await CategoryService.GetForumCategoryFullInfoAsync(categoryId);
            return ApiSuccessResponses.WithData("Get category full info successful", res);
        }

        [HttpGet("categories")]
        public async Task<ApiSuccessResponse<List<CategoryBasicInfo>>> GetCategories()
        {
            var user = AuthService.GetForumUserContext(User);
            var res = await CategoryService.GetForumCategoriesAsync();
            return ApiSuccessResponses.WithData("Get categories successful", res);
        }

        [HttpPost("categories/create")]
        public async Task<ApiSuccessResponse<List<CategoryBasicInfo>>> CreateCategory(CreateCategoryRequest request)
        {
            var user = AuthService.GetForumUserContext(User);
            var res = await CategoryService.CreateForumCategoryAsync(request);
            return ApiSuccessResponses.WithData("Create category successful", res);
        }

        [HttpGet("categories/{categoryId}")]
        public async Task<ApiSuccessResponse<CategoryBasicInfo>> GetSelectedCategory(string categoryId)
        {
            var user = AuthService.GetForumUserContext(User);
            var res = await CategoryService.GetSelectedCategoryAsync(categoryId);
            return ApiSuccessResponses.WithData("Get selected category successful", res);
        }
    }
}
