using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using RestApiServer.CommonEnums;
using RestApiServer.Database.Utils;

namespace RestApiServer.Database.Db
{
    [Table("BannedUsers")]
    public class BannedUserEntry
    {
        [Key]
        public string BanId { get; set; } = DbUtils.GenerateUuid();
        public required string UserId { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter<BanType>))]
        public required BanType BanType { get; set; }
        public required string BanReason { get; set; } = "";
        public required DateTime BanExpirationDate { get; set; }
        
    }
}