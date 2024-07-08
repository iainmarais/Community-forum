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
        public string CategoryId { get; set; } = string.Empty;
        public string TopicName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string CreatedByUserId { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }

        [JsonIgnore]
        public List<ThreadEntry> Threads { get; set; } = new();
        [JsonIgnore]
        public UserEntry? CreatedByUser { get; set; }
        [JsonIgnore]
        public CategoryEntry? Category { get; set; }
    }
}