namespace RestApiServer.Endpoints.Dto.Admin
{
    public class AddUserRequest
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string EmailAddress { get; set; }
        public required string RoleId { get; set; }
    }
}