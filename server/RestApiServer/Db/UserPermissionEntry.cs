using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using RestApiServer.Db.Users;

namespace RestApiServer.Db
{
    [Table("UserPermissions")]
    public class UserPermissionEntry
    {
        [Key]
        public required string UserPermissionId { get; set; }
        [ForeignKey("Users")]
        public required string UserId { get; set; }
        [ForeignKey("SystemPermissions")]
        public required string SystemPermissionId { get; set; }
        [JsonIgnore]
        public UserEntry User { get; set; } = null!;
        [JsonIgnore]
        public SystemPermissionEntry SystemPermission { get; set; } = null!; 

    }
}    