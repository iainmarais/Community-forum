using Microsoft.AspNetCore.Mvc;
using RestApiServer.Core.ApiResponses;
using RestApiServer.Dto.Forum;
using RestApiServer.Services.Gallery;
using RestApiServer.Utils;

namespace RestApiServer.Controllers.Gallery
{

    [ApiController]
    [Route("v1/gallery")]
    public class GalleryController : ControllerBase
    {

        [HttpGet("items")]
        public async Task<ApiSuccessResponse<List<GalleryItemBasicInfo>>> GetGalleryItems()
        {
            var user = AuthUtils.GetForumUserContext(User);
            var res = await GalleryService.GetGalleryItemsAsync();
            return ApiSuccessResponses.WithData("Get gallery items successful", res);
        }

        [HttpGet("items/{itemId}")]
        public async Task<ApiSuccessResponse<GalleryItemBasicInfo>> GetGalleryItem(string itemId)
        {
            var user = AuthUtils.GetForumUserContext(User);
            var res = await GalleryService.GetGalleryItemInfoAsync(itemId);
            return ApiSuccessResponses.WithData("Get gallery item successful", res);
        }
    }
}