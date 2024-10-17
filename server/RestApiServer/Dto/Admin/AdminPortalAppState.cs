using RestApiServer.Db.Users;
using RestApiServer.Dto.App;

namespace RestApiServer.Dto.Admin
{
    public class AdminPortalAppState
    {
        public required LoggedInUserInfo CurrentLoggedInUser { get; set; }
        public required AdminPortalStats AdminPortalStats { get; set; }
    }
}