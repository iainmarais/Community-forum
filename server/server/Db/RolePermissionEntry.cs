using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestApiServer.Db
{
    [Table("RolePermissions")]
    public class RolePermissionEntry
    {
        [Key]
        public required string RolePermissionId { get; set; } = string.Empty;
        public required string RoleId { get; set; } = string.Empty;
        public required string SystemPermissionId { get; set; } = string.Empty;
    }
}