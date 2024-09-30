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
        public required string PermissionName { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter<SystemPermissionType>))]
        public SystemPermissionType Permission { get; set; }
        public string? Description { get; set; }
        //Role with these permissions - set them to false by default
        public bool IsAdminPermission { get; set; } = false;
        public bool IsModeratorPermission { get; set; } = false;
        public bool IsUserPermission { get; set; } = false;
        public bool IsGuestPermission { get; set; } = false;
    }
}