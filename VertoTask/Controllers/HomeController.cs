using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using VertoTask.Data;
using VertoTask.Models;

namespace VertoTask.Controllers
{   
    public class HomeController : Controller
    {
        private readonly ApplicationDBContext _dbContext;
        public HomeController(ApplicationDBContext dBContext)
        {
            _dbContext = dBContext;   
        }

        public async Task<IActionResult> Index()
        {
           var homeViewModel = new HomeViewModel {
                LatestNews = await _dbContext.Newss.FirstOrDefaultAsync(n => n.IsActive),
                LatestGalleryImage = await _dbContext.GalleryImages.FirstOrDefaultAsync(n => n.IsActiveForLatestProduct),
                LatestFieldEvent = await _dbContext.FieldEvents.FirstOrDefaultAsync(n => n.IsActive),
                NewProduct = await _dbContext.Products.FirstOrDefaultAsync(n=> n.IsForSingleProductDisplay),
                SliderProducts = await _dbContext.Products.Where(n => n.IsForProductOfferSlider).ToListAsync(),
                BannerGalleryImages = await _dbContext.GalleryImages.Where(n => n.IsActive).ToListAsync()
           };
            return View(homeViewModel);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}