using Microsoft.EntityFrameworkCore;
using VertoTask.Models;

namespace VertoTask.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext>options): base(options)
        {  
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<FieldEvent> FieldEvents { get; set; }

        public DbSet<News> Newss { get; set; }

        public DbSet<GalleryImage> GalleryImages { get; set; }
    }
}
