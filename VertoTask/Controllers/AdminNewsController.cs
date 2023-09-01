using Microsoft.AspNetCore.Mvc;
using VertoTask.Data;
using Microsoft.EntityFrameworkCore;
using VertoTask.Models;
using System.Diagnostics;

namespace VertoTask.Controllers
{
    public class AdminNewsController : Controller
    {
        private readonly ApplicationDBContext _dbContext;
        private const string UploadsFolder = "wwwroot/images/uploads";

        private const int MaxImageSize = 5 * 1024 * 1024;
        public AdminNewsController(ApplicationDBContext dBContext)
        {
            _dbContext = dBContext;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _dbContext.Newss.ToListAsync());
        }

       
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(News newsItem, IFormFile imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                if (imageFile.Length < MaxImageSize && (imageFile.ContentType == "image/jpeg" || imageFile.ContentType == "image/png"))
                {
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageFile.FileName);
                    var imagePath = Path.Combine(UploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    newsItem.NewsPictureURL = "/images/uploads/" + uniqueFileName;
                }
                else
                {
                    ModelState.AddModelError("ImageError", "Invalid image size or type.");
                    return View(newsItem);
                }
            }

            _dbContext.Newss.Add(newsItem);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var newsItem = _dbContext.Newss.Find(id);
            if (newsItem == null)
            {
                return NotFound();
            }
            return View(newsItem);
        }

        [HttpPost, ActionName("Edit")]
        public async Task<IActionResult> Edit(int id, News newsItem, IFormFile newImageFile)
        {
            if (id != newsItem.Id)
            {
                return NotFound();
            }
            var itemEdit = await _dbContext.Newss.FindAsync(newsItem.Id);
            if (itemEdit != null) {
                itemEdit.Id = newsItem.Id;
                itemEdit.NewsHeading = newsItem.NewsHeading;                
                itemEdit.NewsPublishDate = newsItem.NewsPublishDate;
                if (newImageFile != null && newImageFile.Length > 0)
                {
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(newImageFile.FileName);
                    var imagePath = Path.Combine(UploadsFolder, uniqueFileName);
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await newImageFile.CopyToAsync(stream);
                    }


                    itemEdit.NewsPictureURL = "/images/uploads/" + uniqueFileName;
                }
                itemEdit.NewsWrittenBy = newsItem.NewsWrittenBy;
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
            
        }


        [HttpGet]
        public IActionResult Delete(int id)
        {
            var newsItem = _dbContext.Newss.Find(id);
            if (newsItem == null)
            {
                return NotFound();
            }
            return View(newsItem);
        }

        
        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var newsItem = _dbContext.Newss.Find(id);
            if (newsItem != null)
            {
                _dbContext.Newss.Remove(newsItem);
                await _dbContext.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Activate(int id)
        {
            var newsItem = await _dbContext.Newss.FindAsync(id);
            if (newsItem == null) return NotFound();

            newsItem.IsActive = true;
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Deactivate(int id)
        {
            var newsItem = await _dbContext.Newss.FindAsync(id);
            if (newsItem == null) return NotFound();

            newsItem.IsActive = false;
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}

