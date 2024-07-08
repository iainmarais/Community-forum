using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RestApiServer.Db
{
    [Table("Messages")]
    public class MessageEntry
    {
        [Key]
        public string MessageId { get; set; } = string.Empty;
        public string ThreadId { get; set; } = string.Empty;
        public bool IsFirstPost { get; set; } = false; //Set True if first post. Will be used to remove this entry from the replies count in the forum stats.
        public string CreatedByUserId { get; set; } = string.Empty;
        public string MessageContent { get; set; } = string.Empty;
        public  DateTime CreatedDate { get; set; }

        //Navigation property
        [JsonIgnore]
        public ThreadEntry? Thread { get; set; }
    }
}