using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VertoTask.Data;
using VertoTask.Models;

namespace VertoTask.Controllers
{
    public class AdminGalleryImageController : Controller
    {
        private readonly ApplicationDBContext _dbContext;
        private const string UploadsFolder = "wwwroot/images/uploads";
        private const int MaxImageSize = 10 * 1024 * 1024; //high quality

        public AdminGalleryImageController(ApplicationDBContext dBContext)
        {
            _dbContext = dBContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _dbContext.GalleryImages.ToListAsync());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(GalleryImage galleryImage, IFormFile imageFile)
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

                    galleryImage.ImageUrl = "/images/uploads/" + uniqueFileName;
                }
                else
                {
                    ModelState.AddModelError("ImageError", "Invalid image size or type.");
                    return View(galleryImage);
                }
            }

            _dbContext.GalleryImages.Add(galleryImage);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var galleryImageItem = _dbContext.GalleryImages.Find(id);
            if (galleryImageItem == null)
            {
                return NotFound();
            }
            return View(galleryImageItem);
        }


        [HttpPost, ActionName("Edit")]
        public async Task<IActionResult> Edit(int id, GalleryImage galleryImage, IFormFile newImageFile)
        {
            if (id != galleryImage.Id)
            {
                return NotFound();
            }
            var itemEdit = await _dbContext.GalleryImages.FindAsync(galleryImage.Id);
            if (itemEdit != null)
            {
                itemEdit.Id = galleryImage.Id;
                itemEdit.Title = galleryImage.Title;
                itemEdit.TakenBy = galleryImage.TakenBy;
                itemEdit.Description = galleryImage.Description;
                if (newImageFile != null && newImageFile.Length > 0)
                {
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(newImageFile.FileName);
                    var imagePath = Path.Combine(UploadsFolder, uniqueFileName);
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await newImageFile.CopyToAsync(stream);
                    }


                    itemEdit.ImageUrl = "/images/uploads/" + uniqueFileName;
                }

                await _dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");

        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var galleryImageItem = _dbContext.GalleryImages.Find(id);
            if (galleryImageItem == null)
            {
                return NotFound();
            }
            return View(galleryImageItem);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var galleryImageItem = _dbContext.GalleryImages.Find(id);
            if (galleryImageItem != null)
            {
                _dbContext.GalleryImages.Remove(galleryImageItem);
                await _dbContext.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Activate(int id)
        {
            var galleryImage = await _dbContext.GalleryImages.FindAsync(id);
            if (galleryImage == null) return NotFound();

            galleryImage.IsActive = true;
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Deactivate(int id)
        {
            var galleryImage = await _dbContext.GalleryImages.FindAsync(id);
            if (galleryImage == null) return NotFound();

            galleryImage.IsActive = false;
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> ActivateForSingle(int id)
        {
            var galleryImage = await _dbContext.GalleryImages.FindAsync(id);
            if (galleryImage == null) return NotFound();

            galleryImage.IsActiveForLatestProduct = true;
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeactivateForSingle(int id)
        {
            var galleryImage = await _dbContext.GalleryImages.FindAsync(id);
            if (galleryImage == null) return NotFound();

            galleryImage.IsActiveForLatestProduct = false;
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }



    }
}
