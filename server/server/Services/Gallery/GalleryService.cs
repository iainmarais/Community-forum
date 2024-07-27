using Microsoft.EntityFrameworkCore;
using RestApiServer.Db;
using RestApiServer.Dto.Forum;

namespace RestApiServer.Services.Gallery
{
    public class GalleryService
    {
        public static async Task<List<GalleryItemBasicInfo>> GetGalleryItemsAsync()
        {
            using var db = new AppDbContext();
            var items = await db.GalleryItems
                .Select(i => new GalleryItemBasicInfo(i))
                .ToListAsync();
            return items;
        }

        public static async Task<GalleryItemBasicInfo> GetGalleryItemInfoAsync(string itemId)
        {
            using var db = new AppDbContext();
            var item = await db.GalleryItems
                .Select(i => new GalleryItemBasicInfo(i))
                .SingleOrDefaultAsync(i => i.GalleryItemId == itemId);
            return item ?? throw new Exception("Item not found");
        }
    }
}