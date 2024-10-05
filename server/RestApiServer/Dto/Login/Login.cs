using RestApiServer.Db.Users;
using RestApiServer.Dto.App;

namespace RestApiServer.Dto.Login
{
    //Seems pointless having multiple login responses. One is sufficient. If necessary at a later stage, add a context field, eg. admin, user, etc.
    public class UserLoginResponse
    {
        public required string AccessToken { get; set; } = "";
        public required long AccessTokenExpiration { get; set; }
        public required string RefreshToken { get; set; } = "";
        public required UserBasicInfo UserProfile { get; set; }
    }

    public class UserRefreshResponse
    {
        public required string NewAccessToken { get; set; } = "";
        public required long NewAccessTokenExpiration { get; set; }
        public required string RefreshToken { get; set; } = "";
        public required UserBasicInfo UserProfile { get; set; }
    }

    public class UserLoginRequest
    {
        public required string UserIdentifier { get; set; }
        public required string Password { get; set;}
    }

    public class UserRegistrationRequest
    {
        public required string Username { get; set; }
        public required string EmailAddress { get; set; }
        public required string Password { get; set; }
        public required string RetypePassword { get; set; }
        public string? RoleType { get; set; } = "User";
    }

    public class UserRefreshRequest
    {
        public required string OldAccessToken { get; set; }
        public required string UserRefreshToken { get; set; }
    }
}