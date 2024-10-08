using Microsoft.EntityFrameworkCore;
using RestApiServer.Core.ApiResponses;
using RestApiServer.Db;
using RestApiServer.Dto.Forum;

namespace RestApiServer.Services.Gallery
{
    public class GalleryService
    {
        //Do we paginate this or build a paginated data getter around it?
        public static async Task<List<GalleryItemBasicInfo>> GetGalleryItemsAsync()
        {
            using var db = new AppDbContext();
            var items = await db.GalleryItems
                .Select(i => new GalleryItemBasicInfo()
                {
                    GalleryItem = i,
                    ImageData = GetFileAsync(SplitString(i.GalleryItemLink, '/').Last()).Result
                })
                .ToListAsync();
            return items;
        }

        public static async Task<GalleryItemBasicInfo> GetGalleryItemInfoAsync(string itemId)
        {
            using var db = new AppDbContext();
            var item = await db.GalleryItems
                .Select(i => new GalleryItemBasicInfo()
                {
                    GalleryItem = i,
                    ImageData = GetFileAsync(SplitString(i.GalleryItemLink, '/').Last()).Result
                })
                .SingleOrDefaultAsync(i => i.GalleryItem.GalleryItemId == itemId);
            return item ?? throw new Exception("Item not found");
        }

        public static async Task<List<GalleryItemBasicInfo>> CreateGalleryItemAsync(string userId, CreateGalleryItemRequest request, string filePath)
        {
            using var db = new AppDbContext();
            var item = new GalleryItemEntry
            {
                GalleryItemId = Guid.NewGuid().ToString(),
                CreatedByUserId = userId,
                GalleryItemName = request.GalleryItemName,
                GalleryItemDescription = request.GalleryItemDescription,
                GalleryItemLink = filePath
            };
            await db.GalleryItems.AddAsync(item);
            await db.SaveChangesAsync();
            return await GetGalleryItemsAsync();
        }

        public static async Task<ApiFileResponse> GetFileAsync(string fileName)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            var uploadsFolderName = Path.GetFileName(uploadsFolder);
            var filePath = Path.Combine(uploadsFolder, fileName);

            if (!File.Exists(filePath))
            {
                throw new Exception("File not found");
            }
            var fileBytes = await File.ReadAllBytesAsync(filePath);
            var contentType = GetContentType(filePath);

            var response = new ApiFileResponse
            {   
                ResponseMessage = "Get file successful",
                FileContents = fileBytes,
                ContentType = contentType,
                FileDownloadName = fileName, 
                FileName = $"{uploadsFolderName}/{fileName}"
            };
            return response;
        }
        private static string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types.ContainsKey(ext) ? types[ext] : "application/octet-stream";
        }

        private static Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        } 
        //Since one can't use .Split in expression trees, this helps aleviate that problem.       
        private static List<string> SplitString(string src, char sep)
        {
            return src.Split(sep).Select(x => x.Trim()).ToList();
        }
    }
}