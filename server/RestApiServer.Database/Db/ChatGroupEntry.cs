

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestApiServer.Db
{   
    [Table("ChatGroups")]
    public class ChatGroupEntry
    {
        [Key]
        public string ChatGroupId { get; set; } = string.Empty;
        public string ChatGroupName { get; set; } = string.Empty;
        public string ChatGroupDescription { get; set; } = string.Empty;
    }
}

//sql to create this table