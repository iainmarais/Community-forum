using RestApiServer.Db.Users;

namespace RestApiServer.Dto.App
{
    public class UserBasicInfo
    {
        public required UserEntry User { get; set; }
        public string UserFullname { get => $"{User.UserFirstname} {User.UserLastname}"; }
    }
    public class UserFullInfo
    {
        public required UserEntry User { get; set; }
        //Note to self - Fix this to use contact info. Users should not see other users' passwords or anything auth-related.
        public List<UserBasicInfo>? Contacts { get; set; }
        public string UserFullname { get => $"{User.UserFirstname} {User.UserLastname}"; }   
    }

    //Used by paginated data for the user information. Should store values for total users, most active users, new ( registered >= 30 days prior ) users, etc
    public class UserSummary
    {
        //Default to 0.
        public int TotalUsers { get; set; } = 0;
        public int MostActiveUsers { get; set; } = 0;
        public int NewUsers { get; set; } = 0;
    }
}