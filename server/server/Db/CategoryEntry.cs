using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RestApiServer.Db
{
    [Table("Categories")]
    public class CategoryEntry
    {
        [Key]
        public string CategoryId { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string CategoryDescription { get; set; } = string.Empty;

        //Navigation properties
        [JsonIgnore]
        public List<TopicEntry> TopicsCreated { get; set; } = new();

    }

}