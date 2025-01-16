using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using RestApiServer.Db.Users;

namespace RestApiServer.Db
{
    [Table("Topics")]
    public class TopicEntry
    {
        [Key]
        public string TopicId { get; set; } = string.Empty;
        public string BoardId { get; set; } = string.Empty;
        public string TopicName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string CreatedByUserId { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public bool IsMarkedForDelete { get; set; } = false;
        public DateTime DateMarkedForDelete { get; set; }
        public bool IsImportant { get; set; } = false;

        [JsonIgnore]
        public List<ThreadEntry> Threads { get; set; } = new();
        [JsonIgnore]
        public UserEntry? CreatedByUser { get; set; }
        [JsonIgnore]
        public BoardEntry? Board { get; set; }
    }
}