using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestApiServer.Db
{
    [Table("UserRefreshTokens")]
    public class UserRefreshTokenEntry
    {
        [Key]
        public required string UserRefreshTokenId { get; set; }
        public required string UserId { get; set; }
        public required string RefreshToken { get; set; }
        public required DateTime RefreshTokenExpirationDate { get; set; }
        public required string Source { get; set; }
    }
}