using System.ComponentModel.DataAnnotations;

namespace VertoTask.Models
{
    public class FieldEvent
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string EventName { get; set; }
        [Required]
        public DateTime EventDate { get; set; }
        [Required]
        public string Location { get; set; }
        public string Description { get; set; }

        public bool IsActive { get; set; } = true;

    }

}
