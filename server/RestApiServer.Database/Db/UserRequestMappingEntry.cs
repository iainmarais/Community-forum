using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RestApiServer.Db.Users;

namespace RestApiServer.Db.UserRequestMapping
{
    [Table("UserRequestMappings")]
    public class UserRequestMappingEntry
    {
        [Key]
        public string UserMappingId { get; set; } = string.Empty;
        [ForeignKey("UserEntry")]
        public string UserId { get; set; } = string.Empty;
        public UserEntry User = new();
        [ForeignKey("RequestEntry")]
        public string RequestId { get; set; } = string.Empty;
        public RequestEntry Request = new();
        //Boolean properties for assiging states representing assigned, resolved, closed, etc.
        public bool IsCreator { get; set; } = false; //True if user is the creator
        public bool IsAssigned { get; set; } = false; //True if user is assigned to the request
        public bool IsResolved { get; set; } = false; //True if user has resolved the request
        public bool IsClosed { get; set; } = false; //True if user has closed the request
        public DateTime DateAssigned { get; set; }
    }
}