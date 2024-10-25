namespace RestApiServer.Endpoints.Dto.Admin
{
    public class CreateCategoryRequest
    {
        public string CategoryName { get; set; } = "";
        public string CategoryDescription { get; set; } = "";
    }
}