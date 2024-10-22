using RestApiServer.Database.Db;
using RestApiServer.Db.Users;

namespace RestApiServer.Endpoints.Dto.Admin
{
    public class BannedUserBasicInfo
    {
        public required BannedUserEntry BannedUser { get; set; }
        public UserEntry User { get; set; } = null!;
    }


    public class BannedUserSummary
    {
        public int TotalBannedUsers { get; set; }
        public int PermanentlyBannedUsers { get; set; }
    }
}