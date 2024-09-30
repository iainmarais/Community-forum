using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RestApiServer.Db
{
    [Table("Permissions")]
    public class PermissionEntry
    {
        [Key]
        public required string PermissionId { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public required string PermissionName { get; set; }
        public required string Description { get; set; }
    }
}