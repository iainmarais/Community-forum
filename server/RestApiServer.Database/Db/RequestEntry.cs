using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using RestApiServer.CommonEnums;
using RestApiServer.Db.Users;

namespace RestApiServer.Db
{
    [Table("Requests")]
    public class RequestEntry
    {
        [Key]
        public required string RequestId { get; set; }
        //Id of the user
        public string CreatedByUserId { get; set; } = string.Empty;
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
        public DateTime? DateUpdated { get; set; }
        public DateTime? DateResolved { get; set; }
        public bool IsMarkedForDelete { get; set; } = false;
        public DateTime DateMarkedForDelete { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter<TriageStatus>))]
        public TriageStatus TriageStatus { get; set; } = TriageStatus.Untriaged; //Default
        [JsonConverter(typeof(JsonStringEnumConverter<TriageType>))]
        public TriageType TriageType { get; set; } = TriageType.Unspecified; //Default
        //Navigation props
        [JsonIgnore]
        public UserEntry CreatedByUser { get; set; } = new();
        [JsonIgnore]
        [NotMapped]
        public UserEntry AssignedToUser { get; set; } = new();
        [JsonIgnore]
        [NotMapped]
        public UserEntry ResolvedByUser { get; set; } = new();
        [JsonIgnore]
        [NotMapped]
        public UserEntry LastUpdatedByUser { get; set; } = new(); //The last user to update this request.
    }
}