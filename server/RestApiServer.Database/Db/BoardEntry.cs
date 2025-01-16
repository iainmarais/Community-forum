
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RestApiServer.Db
{
    [Table("Boards")]
    public class BoardEntry 
    {
        [Key]
        public string BoardId { get; set; } = string.Empty;
        public string CategoryId { get; set; } = string.Empty;
        public string CreatedByUserId { get; set; } = string.Empty;
        public string BoardName { get; set; } = string.Empty;
        public string BoardDescription { get; set; } = string.Empty;
        public bool IsMarkedForDelete { get; set; } = false;
        public DateTime DateMarkedForDelete { get; set; }
        public bool IsImportant { get; set; } = false;        

        //Navigation props
        [JsonIgnore]
        public List<TopicEntry> TopicsCreated { get; set; } = null!;
        [JsonIgnore]
        public CategoryEntry? Category { get; set; } = null!;
    }
}