using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using RestApiServer.Utils;

namespace RestApiServer.Db.Users
{
    [Table("Users")]
    public class UserEntry
    {
        [Key]
        public string UserId { get; set; } = string.Empty;
        public string AdminUserId { get; set; } = string.Empty;
        public string ForumUserId { get; set; } = string.Empty;
        public string UserProfileImageBase64 { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? CityName { get; set; }
        public string? CountryName { get; set; }
        public string? PostalCode { get; set; }
        public string UserFirstname { get; set; } = string.Empty;
        public string UserLastname { get; set; } = string.Empty;
        public string? Gender { get; set; } //Male, female or other. FYI I do not subscribe to wokery, so therefore I will not and shall not cater to it.
        public string HashedPassword { get; set; } = string.Empty;
        public string RoleId { get; set; } = string.Empty;
        public int? TotalPosts { get; set; }
        //Chat-related
        public bool IsOnline { get; set; } //Typically set by the frontend when the user is logged in. Polling this could be tricky - one needs to use a realtime service such as signalr to keep it up to date
        public bool IsVisible { get; set; } //Controlled by the user from the chat view. If set to false, the user will not be visible in the chat view.
        //Use this to keep track of the last successful login - can be used for polling to see what has changed since last login. 
        //Updating it should be run every day or so, depending on the user's current state
        public DateTime LastLoginTime { get; set; }

        //Navigation properties
        [JsonIgnore]
        public UserEntry User { get; set; } = new();
        [JsonIgnore]
        public List<ThreadEntry> ThreadsCreated { get; set; } = new();
        [JsonIgnore]
        public List<TopicEntry> TopicsCreated { get; set; } = new();
        [JsonIgnore]
        public List<GalleryItemEntry> GalleryItems { get; set; } = new();
        [JsonIgnore]       
        public List<UserPermissionEntry> UserPermissions { get; set; } = new(); 
        [JsonIgnore]
        public List<UserRefreshTokenEntry> UserRefreshTokens { get; set; } = new();

        //Guest placeholder user
        public static UserEntry CreateDefaultGuestUser()
        {
            return new UserEntry()
            {
                UserId = "Guest",
                Username = "Guest",
                EmailAddress = "Guest",
                HashedPassword = BCrypt.Net.BCrypt.HashPassword("Guest"),
                RoleId = "Guest"
            };
        }
    }

    public class LoggedInUserInfo
    {
        public required string UserId { get; set; }
        public string UserFirstname { get; set; } = "";
        public string UserLastname { get; set; } = "";
        public string UserProfileImageBase64 { get; set; } = "";
        public string RoleName { get; set; } = "";
    }
}