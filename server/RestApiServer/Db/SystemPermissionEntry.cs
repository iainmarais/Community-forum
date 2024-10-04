using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using RestApiServer.Enums;
using RestApiServer.Utils;

namespace RestApiServer.Db
{
    [Table("SystemPermissions")]
    public class SystemPermissionEntry
    {
        [Key]
        public required string SystemPermissionId { get; set; }
        public required string SystemPermissionName { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter<SystemPermissionType>))]
        public SystemPermissionType SystemPermissionType { get; set; }
        public string? Description { get; set; }
        [JsonIgnore]
        public List<UserPermissionEntry> UserPermissions { get; set; } = new();
    }
}