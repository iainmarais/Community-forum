using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestApiServer.Db
{

    [Table("GalleryItems")]
    public class GalleryItemEntry
    {
        [Key]
        public string GalleryItemId { get; set; } = string.Empty; //Primary key, varchar(50) is large enough
        public string CreatedByUserId { get; set; } = string.Empty;
        public string GalleryItemName { get; set; } = string.Empty; //varchar(50)
        public string GalleryItemDescription { get; set; } = string.Empty; //Medium text.
        public string GalleryItemLink { get; set; } = string.Empty; //The url of the image, varchar(255) should do it.
        public int NumLikes { get; set; } = 0; //int(10)
        public int NumDislikes { get; set; } = 0; //int(10)
        public DateTime CreatedDate { get; set; } = DateTime.Now; 
    }
}
