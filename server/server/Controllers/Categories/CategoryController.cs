using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestApiServer.Core.ApiResponses;
using RestApiServer.Dto.App;
using RestApiServer.Dto.Forum;
using RestApiServer.Services.Categories;
using RestApiServer.Utils;

namespace RestApiServer.Controllers.Categories
{

    [ApiController]
    [Route("v1/forum")]
    public class CategoryController : ControllerBase
    {

        [HttpGet("categories/{categoryId}/fullinfo")]
        public async Task<ApiSuccessResponse<CategoryFullInfo>> GetCategoryFullInfo(string categoryId)
        {
            var user = AuthUtils.GetForumUserContext(User);
            var res = await CategoryService.GetForumCategoryFullInfoAsync(categoryId);
            return ApiSuccessResponses.WithData("Get category full info successful", res);
        }

        [HttpGet("categories")]
        public async Task<ApiSuccessResponse<List<CategoryBasicInfo>>> GetCategories()
        {
            var user = AuthUtils.GetForumUserContext(User);
            var res = await CategoryService.GetForumCategoriesAsync();
            return ApiSuccessResponses.WithData("Get categories successful", res);
        }

        [HttpPost("categories/create")]
        public async Task<ApiSuccessResponse<List<CategoryBasicInfo>>> CreateCategory(CreateCategoryRequest request)
        {
            var user = AuthUtils.GetForumUserContext(User);
            var res = await CategoryService.CreateForumCategoryAsync(request);
            return ApiSuccessResponses.WithData("Create category successful", res);
        }
    }
}
