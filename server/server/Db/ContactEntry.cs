
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RestApiServer.Db
{

    [Table("Contacts")]
    public class ContactEntry
    {
        [Key]
        public string ContactId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty; //Refers to the user associated with this contact. These two should be the same value.
        public string CreatedByUserId { get; set; } = string.Empty; //This is the user who created the contact
        public string ContactName { get; set; } = string.Empty;
        public string ContactEmailAddress { get; set; } = string.Empty;
        public string AboutMessage { get; set; } = string.Empty;
        public string ContactProfileImageBase64 { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}