using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RestApiServer.Db
{

    [Table("Chats")]
    public class ChatEntry
    {
        [Key]
        public string ChatId { get; set; } = string.Empty;
        public string ChatGroupId { get; set; } = string.Empty; //For group chats. Will create a dedicated ChatGroup dbset and table when I get round to building out this functionality.
        public string ChatName { get; set; } = string.Empty;
        public string CreatedByUserId { get; set; } = string.Empty;
        public string SecondParticipantUserId { get; set; } = string.Empty; //Represents the second participant.
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}