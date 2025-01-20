namespace RestApiServer.Dto.Admin
{
    public class AdminPortalUserRegistrationRequest
    {
        public required string Username { get; set; }
        public required string EmailAddress { get; set; }
        public required string Password { get; set; }
        public required string RetypePassword { get; set; }
        public string? RoleType { get; set; } = "User";
    }
}