
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestApiServer.Db
{

    [Table("ChatMessages")]
    public class ChatMessageEntry
    {
        [Key]
        public string ChatMessageId { get; set; } = string.Empty;
        public string ChatGroupId { get; set; } = string.Empty; //This should not be set to any value unless the message forms part of a group chat.
        public string CreatedByUserId { get; set; } = string.Empty;
        public string RecipientUserId { get; set; } = string.Empty;
        public string ChatMessageContent { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public bool IsEdited { get; set; } = false;
        public DateTime EditedTime { get; set; } //Show this if the message has been edited. It should show the time as well.
    }
}
