namespace RestApiServer.Dto.Forum
{
    public class CreateGalleryItemRequest
    {
        public required string GalleryItemName { get; set; } = "";
        public required string GalleryItemDescription { get; set; } = "";
    }
}