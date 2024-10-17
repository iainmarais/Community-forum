using RestApiServer.Dto.App;

namespace RestApiServer.Dto.AdminLogin
{
    public class AdminUserLoginResponse
    {
        public required string AccessToken { get; set; } = "";
        public required long AccessTokenExpiration { get; set; }
        public required string AdminUserRefreshToken { get; set; } = "";
        public required UserBasicInfo AdminUserProfile { get; set; }
    }
}