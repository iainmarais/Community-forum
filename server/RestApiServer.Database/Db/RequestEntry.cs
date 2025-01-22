using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using RestApiServer.CommonEnums;
using RestApiServer.Db.UserRequestMapping;
using RestApiServer.Db.Users;

namespace RestApiServer.Db
{
    [Table("Requests")]
    public class RequestEntry
    {
        [Key]
        public string RequestId { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        [StringLength(100, MinimumLength = 10)] //Don't post empty request titles. Say something or the system will drop your request
        
        //At a later stage, if this grows more complex it might help to create this as a separate table to contain the data, but I don't foresee that happening yet.
        public string SupportRequestTitle { get; set; } = string.Empty;
        [StringLength(1000, MinimumLength =20)] //No posting of meaningless requests to the system. 20 chars minimum should do to prevent stupid messages being posted.
        public string SupportRequestContent { get; set; } = string.Empty;
        public bool IsMarkedForDelete { get; set; } = false;
        public DateTime DateMarkedForDelete { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter<TriageStatus>))]
        public TriageStatus TriageStatus { get; set; } = TriageStatus.Untriaged; //Default
        [JsonConverter(typeof(JsonStringEnumConverter<TriageType>))]
        public TriageType TriageType { get; set; } = TriageType.Unspecified; //Default
        //Navigation props
        [JsonIgnore]
        [NotMapped] //Experience and evidence tells me the [NotMapped] directive is not necessary for collections, but I'm not taking any chances now.
        public List<UserRequestMappingEntry> UserRequestMappings { get; set; } = new();
        [JsonIgnore]
        [NotMapped]
        public UserEntry CreatedByUser { get; set; } = new();
        [JsonIgnore]
        [NotMapped]
        public UserEntry AssignedToUser { get; set; } = new();
        [JsonIgnore]
        [NotMapped]
        public UserEntry ResolvedByUser { get; set; } = new();
    }
}