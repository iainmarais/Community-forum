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
        public string? UserFirstname { get; set; }
        public string? UserLastname { get; set; }
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

        /*
        //Create the SQL code to create the database table from this c# class, ignore navigation props.
        create table if not exists users (
            userId varchar(50) primary key,
            adminUserId varchar(50),
            forumUserId varchar(50),
            userProfileImageBase64 varchar(2000),
            username varchar(20),
            emailAddress varchar(50),
            address varchar(50),
            city varchar(50),
            country varchar(50),
            postalCode varchar(10),
            userFirstname varchar(50),
            userLastname varchar(50),
            gender varchar(10),
            hashedPassword varchar(2000),
            roleId varchar(50),
            totalPosts int,
            isOnline bit,
            isVisible bit,
            lastLoginTime datetime
        )

        */
        //Navigation properties
        [JsonIgnore]
        public List<ThreadEntry> ThreadsCreated { get; set; } = new();
        [JsonIgnore]
        public List<TopicEntry> TopicsCreated { get; set; } = new();

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