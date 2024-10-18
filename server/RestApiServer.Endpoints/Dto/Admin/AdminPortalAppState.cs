using RestApiServer.Db.Users;

namespace RestApiServer.Dto.Admin
{
    public class AdminPortalAppState
    {
        public required LoggedInUserInfo CurrentLoggedInUser { get; set; }
        public required AdminPortalStats AdminPortalStats { get; set; }
    }
}