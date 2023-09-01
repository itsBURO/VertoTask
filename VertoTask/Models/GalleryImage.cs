using System.ComponentModel.DataAnnotations;

namespace VertoTask.Models
{
    public class GalleryImage
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        [Required]
        public string TakenBy { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        public string Description { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsActiveForLatestProduct { get; set; } = false;
    }
}
