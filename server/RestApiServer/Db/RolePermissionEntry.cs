using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RestApiServer.Db
{
    [Table("RolePermissions")]
    public class RolePermissionEntry
    {
        [Key]
        public required string RolePermissionId { get; set; } = Guid.NewGuid().ToString();
        [ForeignKey("Roles")]
        public required string RoleId { get; set; } = string.Empty;
        //Foreign key - Permissions table -> PermissionId column.
        [ForeignKey("Permissions")]
        public required string PermissionId { get; set; } = string.Empty;
        public required bool IsAllowed { get; set; } = false; //Default deny. Screw you, would-be hackers. No rights? No access!
        //Navigation properties:
        [JsonIgnore]
        public RoleEntry Role { get; set; } = null!;
        [JsonIgnore]
        public PermissionEntry Permission { get; set; } = null!;
    }
}