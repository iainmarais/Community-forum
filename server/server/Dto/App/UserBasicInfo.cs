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
        public List<UserBasicInfo> Contacts { get; set; }
        public string UserFullname { get => $"{User.UserFirstname} {User.UserLastname}"; }   
    }
}