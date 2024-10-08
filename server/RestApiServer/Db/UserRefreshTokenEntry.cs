using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using RestApiServer.Db.Users;

namespace RestApiServer.Db
{
    [Table("UserRefreshTokens")]
    public class UserRefreshTokenEntry
    {
        [Key]
        public required string UserRefreshTokenId { get; set; }
        [ForeignKey("AssignedToUserId")]
        public required string AssignedToUserId { get; set; }
        public required string RefreshToken { get; set; }
        public required DateTime RefreshTokenExpirationDate { get; set; }
        public required string Source { get; set; }
        public required bool IsRevoked { get; set; }
        [JsonIgnore]
        public UserEntry User { get; set; } = null!;
    }
}