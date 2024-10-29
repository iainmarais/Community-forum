namespace RestApiServer.Endpoints.Dto.Admin
{
    public class AdminCreateCategoryRequest
    {
        public string CategoryName { get; set; } = "";
        public string CategoryDescription { get; set; } = "";
    }
}