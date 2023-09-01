using System.ComponentModel.DataAnnotations;

namespace VertoTask.Models
{
    public class News
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string NewsHeading { get; set; }
        public DateTime NewsPublishDate { get; set; } = DateTime.Now;
        [Required]
        public string NewsWrittenBy { get; set; }
        [Required]
        public string NewsReport { get; set; }
        public string NewsPictureURL { get; set; }

        public bool IsActive { get; set; } = true;

    }
}
