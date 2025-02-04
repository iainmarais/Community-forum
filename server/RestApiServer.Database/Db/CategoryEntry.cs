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
        public string? CategoryName { get; set; } // Made optional
        public string? CategoryDescription { get; set; } // Made optional
        public required string CreatedByUserId { get; set; } = string.Empty;
        public bool IsMarkedForDelete { get; set; } = false;
        public DateTime? DateMarkedForDelete { get; set; } // Made optional
        public bool IsImportant { get; set; } = false;
        // Navigation properties
        [JsonIgnore]
        public List<BoardEntry> BoardsCreated { get; set; } = new();
    }
}