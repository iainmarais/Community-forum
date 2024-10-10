using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestApiServer.Core.ApiResponses;
using RestApiServer.Dto.Forum;
using RestApiServer.Services.Forum.Gallery;
using RestApiServer.Utils;

namespace RestApiServer.Controllers.Forum.Gallery
{

    [ApiController]
    [Route("v1/gallery")]
    [Authorize]
    public class GalleryController : ControllerBase
    {

        [HttpGet("items")]
        public async Task<ApiSuccessResponse<List<GalleryItemBasicInfo>>> GetGalleryItems()
        {
            var user = AuthUtils.GetForumUserContext(User); //How can I see why this somehow fails?
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

        [HttpPost("items/create")]
        public async Task<ApiSuccessResponse<List<GalleryItemBasicInfo>>> CreateGalleryItem([FromForm] IFormCollection form)
        {
            var user = AuthUtils.GetForumUserContext(User);

            if(form == null)
            {
                throw new Exception("Form not found.");
            }
            string galleryItemName = form["galleryItemName"]!;
            string galleryItemDescription = form["galleryItemDescription"]!;
            IFormFile file = form.Files.GetFile("file")!;

            if (file == null || file.Length == 0)
            {
                throw new Exception("File not found.");
            }

            var fileName = Path.GetFileName(file.FileName);
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }
            var filePath = Path.Combine(uploadsFolder, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var fileLink = $"uploads/{fileName}";

            var request = new CreateGalleryItemRequest
            {
                GalleryItemName = galleryItemName,
                GalleryItemDescription = galleryItemDescription
            };

            var res = await GalleryService.CreateGalleryItemAsync(user.UserId, request, fileLink);
            return ApiSuccessResponses.WithData("Create gallery item successful", res);
        }

        [HttpGet("uploads/{fileName}")]
        public async Task<ApiSuccessResponse<ApiFileResponse>> GetFile(string fileName)
        {
            var fileResponse = await GalleryService.GetFileAsync(fileName);
            return ApiSuccessResponses.WithData("Get file successful", fileResponse);
        }
    }
}