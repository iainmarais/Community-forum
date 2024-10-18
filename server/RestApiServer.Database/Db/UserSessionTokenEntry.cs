using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using RestApiServer.Db.Users;

namespace RestApiServer.Db
{
    [Table("UserSessionTokens")]
    public class UserSessionTokenEntry
    {
        [Key]
        public required string UserSessionTokenId { get; set; }
        public required DateTime DateCreated { get; set; }
        [ForeignKey("AssignedToUserId")]
        public required string AssignedToUserId { get; set; }
        public required bool IsRevoked { get; set; } = false;
        public DateTime? DateRevoked { get; set; }
        [StringLength(8000)]
        public required string SessionToken { get; set; }
        public bool IsExpired => DateExpired < DateTime.UtcNow;
        public required DateTime DateExpired { get; set; }
        public required string Source { get; set; }
        [JsonIgnore]
        public UserEntry User { get; set; } = null!;
    }
}