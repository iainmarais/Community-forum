using RestApiServer.Db.Users;
using RestApiServer.Utils;

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
}