using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using RestApiServer.Db.Users;

namespace RestApiServer.Db
{
    [Table("Threads")]
    public class ThreadEntry
    {
        [Key]
        public string ThreadId { get; set; }  = string.Empty;
        public string ThreadName { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public string TopicId { get; set; } = string.Empty;
        public string CreatedByUserId { get; set; } = string.Empty; //This must only be set when the thread is created, not for any subsequent posts.
        public int NumberOfPosts { get; set; } = 0; //This should increase with each new post.
        public bool HasNewPosts { get; set; } = false; //If the thread has new posts since the last check - this should take into account the user viewing it since it is per user.

        //Navigation props:
        [JsonIgnore]
        public List<MessageEntry> Messages { get; set; } = new();
        [JsonIgnore]
        public UserEntry? CreatedByUser { get; set; }

        [JsonIgnore]
        public TopicEntry? Topic { get; set; }
    }
}