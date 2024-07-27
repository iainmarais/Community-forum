using RestApiServer.Db;
using RestApiServer.Utils;

namespace RestApiServer.Dto.Forum
{
    public class GalleryItemBasicInfo : GalleryItemEntry
    {
        public GalleryItemBasicInfo(GalleryItemEntry entry)
        { 
            ClassUtils.CopyFromBaseclass(this, entry);
        }
    }
}