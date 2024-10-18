namespace RestApiServer.Dto.Admin
{
    public class AdminUserLoginRequest
    {
        public required string UserIdentifier { get; set; }
        public required string Password { get; set;}
        //Default to forum if not set.
        public required string UserContext { get; set; } = "admin";
    }
}