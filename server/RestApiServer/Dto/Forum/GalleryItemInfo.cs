using RestApiServer.Core.ApiResponses;
using RestApiServer.Db;

namespace RestApiServer.Dto.Forum
{
    public class GalleryItemBasicInfo
    {
        public required GalleryItemEntry GalleryItem { get; set; } = null!;
        public required ApiFileResponse ImageData { get; set; } = new ApiFileResponse();
    }
}