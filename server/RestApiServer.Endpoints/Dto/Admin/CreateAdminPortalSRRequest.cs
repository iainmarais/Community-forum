namespace RestApiServer.Dto.Admin
{
    public class CreateAdminPortalSRRequest
    {
        public required string SupportRequestTitle { get; set; }
        public required string SupportRequestContent { get; set; }
    }
}