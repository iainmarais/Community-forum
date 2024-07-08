using RestApiServer.Db.Users;
using RestApiServer.Utils;

namespace RestApiServer.Dto.App
{
    public class UserBasicInfo : UserEntry
    {
        public UserBasicInfo(UserEntry user)
        {
            ClassUtils.CopyFromBaseclass(this, user);
        }

        public string UserFullname { get => $"{UserFirstname} {UserLastname}"; }
    }
}