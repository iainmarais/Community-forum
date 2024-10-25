namespace RestApiServer.Dto.Forum
{
    public class CreateCategoryRequest
    {
        public required string CategoryName { get; set; } = "";
        public required string CategoryDescription { get; set; } = "";
        public required string CreatedByUserId { get; set; } = "";
    }
}