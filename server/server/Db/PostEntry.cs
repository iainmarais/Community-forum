using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RestApiServer.Db
{
    [Table("Posts")]
    public class PostEntry
    {
        [Key]
        public string PostId { get; set; } = string.Empty;
        public string ThreadId { get; set; } = string.Empty;
        public bool IsFirstPost { get; set; } = false; //Set True if first post. Will be used to remove this entry from the replies count in the forum stats.
        public string CreatedByUserId { get; set; } = string.Empty;
        public string PostContent { get; set; } = string.Empty;
        public  DateTime CreatedDate { get; set; }
        public string ReplyToPostId { get; set; } = string.Empty;
        public bool PostReported { get; set; } = false;
        public string ReportReason { get; set; } = string.Empty;
        public string ReportedByUserId { get; set; } = string.Empty;

        //Navigation property
        [JsonIgnore]
        public ThreadEntry? Thread { get; set; }
    }
}