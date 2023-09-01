using System.ComponentModel.DataAnnotations;

namespace VertoTask.Models
{

    
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public string Type { get; set; }  
        [Required]
        public decimal Price { get; set; }
        public string Description { get; set; }
        [Required]
        public string ImageUrl { get; set; }

        public bool IsForProductOfferSlider { get; set; } = false;

        public bool IsForSingleProductDisplay { get; set; } = false;

        public DateTime AddedOn { get; set; } = DateTime.Now;
    }
}
