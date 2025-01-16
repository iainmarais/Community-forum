using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RestApiServer.Db
{
    [Table("Categories")]
    public class CategoryEntry
    {
        [Key]
        public required string CategoryId { get; set; } = string.Empty;
        public required string CategoryName { get; set; } = string.Empty;
        public required string CategoryDescription { get; set; } = string.Empty;
        public required string CreatedByUserId { get; set; } = string.Empty;
        public bool IsMarkedForDelete { get; set; } = false;
        public DateTime DateMarkedForDelete { get; set; }
        public bool IsImportant { get; set; } = false;
        //Navigation properties
        [JsonIgnore]
        public List<BoardEntry> BoardsCreated { get; set; } = new();

    }

}