using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using RestApiServer.CommonEnums;
using RestApiServer.Db.Users;

namespace RestApiServer.Db
{
    [Table("Requests")]
    public class RequestEntry
    {
        [Key]
        public required string SupportRequestId { get; set; }
        //Id of the user
        public required string CreatedByUserId { get; set; }
        public required DateTime CreatedDate { get; set; }
        [StringLength(100, MinimumLength = 10)] //Don't post empty request titles. Say something or the system will drop your request
        
        //At a later stage, if this grows more complex it might help to create this as a separate table to contain the data, but I don't foresee that happening yet.
        public required string SupportRequestTitle { get; set; }
        [StringLength(1000, MinimumLength =20)] //No posting of meaningless requests to the system. 20 chars minimum should do to prevent stupid messages being posted.
        public required string SupportRequestContent { get; set; }
        //These UserId's are set by the server during the process of creating and updating requests.
        public string? AssignedToUserId { get; set; } //UserEntry id of the user this was assigned to, or who self-assigned the request.
        public string? ResolvedByUserId { get; set; } //UserEntry id of the user responsible for resolving the request.
        public string? LastUpdatedByUserId { get; set; } //UserEntry id of the user who last updated this request. 
        public bool IsResolved { get; set; } = false;
        public DateTime? DateResolved { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter<TriageStatus>))]
        public TriageStatus TriageStatus { get; set; } = TriageStatus.Untriaged; //Default
        [JsonConverter(typeof(JsonStringEnumConverter<TriageType>))]
        public TriageType TriageType { get; set; } = TriageType.Unspecified; //Default
        //Navigation props
        [JsonIgnore]
        public UserEntry? CreatedByUser { get; set; }
        [JsonIgnore]
        public UserEntry? AssignedToUser { get; set; }
        [JsonIgnore]
        public UserEntry? ResolvedByUser { get; set; }
        [JsonIgnore]
        public UserEntry? LastUpdatedByUser { get; set; } //The last user to update this request.
    }
}