namespace VertoTask.Models
{
    public class HomeViewModel
    {
        public News LatestNews { get; set; }
        public Product NewProduct { get; set; }
        public GalleryImage LatestGalleryImage { get; set; }
        public FieldEvent LatestFieldEvent { get; set; }
        public List<Product> SliderProducts { get; set; }
        public List<GalleryImage> BannerGalleryImages { get; set; }
    }
}
