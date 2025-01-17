namespace RestApiServer.Dto.Admin
{
    public class CreateAdminPortalRequest
    {
        public required string SupportRequestTitle { get; set; }
        public required string SupportRequestContent { get; set; }
    }
}