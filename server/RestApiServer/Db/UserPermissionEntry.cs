using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestApiServer.Db
{
    [Table("UserPermissions")]
    public class UserPermissionEntry
    {
        [Key]
        public required string UserPermissionId { get; set; }
        public required string UserId { get; set; }
        public required string SystemPermissionId { get; set; }
    }
}    