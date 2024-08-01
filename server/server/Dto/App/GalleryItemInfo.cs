using System.Data;
using RestApiServer.Core.ApiResponses;
using RestApiServer.Db;
using RestApiServer.Utils;

namespace RestApiServer.Dto.Forum
{
    public class GalleryItemBasicInfo
    {
        public required GalleryItemEntry GalleryItem { get; set; } = null!;
        public required ApiFileResponse ImageData { get; set; } = new ApiFileResponse();
    }
}