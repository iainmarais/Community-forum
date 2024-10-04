using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using RestApiServer.Enums;

namespace RestApiServer.Db
{
    [Table("Permissions")]
    public class PermissionEntry
    {
        [Key]
        public required string PermissionId { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public required string PermissionName { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter<PermissionType>))]
        public PermissionType PermissionType { get; set; }
        public required string Description { get; set; }
        [JsonIgnore]
        public required List<RolePermissionEntry> RolePermissions { get; set; } = new();
    }
}