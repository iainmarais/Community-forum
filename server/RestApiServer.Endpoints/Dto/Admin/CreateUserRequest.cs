namespace RestApiServer.Dto.Admin
{
    public class CreateUserRequest
    {
        public required string Username;
        public required string EmailAddress;
        public required string RoleName;
        public required string Password;
        public required string RetypePassword;
    }
}