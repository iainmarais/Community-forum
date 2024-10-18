using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using RestApiServer.CommonEnums;

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

/*
    Note to self:
    System permissions will most likely be used to either control visibility of certain elements, such as for development etc, which can be assigned on a per-user basis.
    Alternatively, these can be associated with a developer role.
*/