using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using RestApiServer.Enums;

namespace RestApiServer.Db
{
    [Table("Roles")]
    public class RoleEntry
    {
        [Key]
        public required string RoleId { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public required string RoleName { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter<RoleType>))]
        public RoleType RoleType { get; set;}
        public string? Description { get; set; }
        [JsonIgnore]
        public required List<RolePermissionEntry> RolePermissions { get; set; } = new();
    }
}