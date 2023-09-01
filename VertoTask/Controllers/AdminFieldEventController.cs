using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VertoTask.Data;
using VertoTask.Models;

namespace VertoTask.Controllers
{
    public class AdminFieldEventController : Controller
    {
        private readonly ApplicationDBContext _dbContext;
        private const string UploadsFolder = "wwwroot/images/uploads";

        private const int MaxImageSize = 5 * 1024 * 1024;
        public AdminFieldEventController(ApplicationDBContext dBContext)
        {
            _dbContext = dBContext;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _dbContext.FieldEvents.ToListAsync());
        }
        [HttpGet]
        public IActionResult Create() {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(FieldEvent fieldEvent, IFormFile imageFile)
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

                    fieldEvent.Location = "/images/uploads/" + uniqueFileName;
                }
                else
                {
                    ModelState.AddModelError("ImageError", "Invalid image size or type.");
                    return View(fieldEvent);
                }
            }

            _dbContext.FieldEvents.Add(fieldEvent);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var fieldEventItem = _dbContext.FieldEvents.Find(id);
            if (fieldEventItem == null)
            {
                return NotFound();
            }
            return View(fieldEventItem);
        }

        [HttpPost, ActionName("Edit")]
        public async Task<IActionResult> Edit(int id, FieldEvent fieldEvent, IFormFile newImageFile)
        {
            if (id != fieldEvent.Id)
            {
                return NotFound();
            }
            var itemEdit = await _dbContext.FieldEvents.FindAsync(fieldEvent.Id);
            if (itemEdit != null)
            {
                itemEdit.Id = fieldEvent.Id;
                itemEdit.EventDate = fieldEvent.EventDate;
                itemEdit.EventName = fieldEvent.EventName;
                itemEdit.Description = fieldEvent.Description;
                if (newImageFile != null && newImageFile.Length > 0)
                {
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(newImageFile.FileName);
                    var imagePath = Path.Combine(UploadsFolder, uniqueFileName);
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await newImageFile.CopyToAsync(stream);
                    }


                    fieldEvent.Location = "/images/uploads/" + uniqueFileName;
                }
                
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");

        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var fieldEventItem = _dbContext.FieldEvents.Find(id);
            if (fieldEventItem == null)
            {
                return NotFound();
            }
            return View(fieldEventItem);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var newsItem = _dbContext.FieldEvents.Find(id);
            if (newsItem != null)
            {
                _dbContext.FieldEvents.Remove(newsItem);
                await _dbContext.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Activate(int id)
        {
            var fieldEventItem = await _dbContext.FieldEvents.FindAsync(id);
            if (fieldEventItem == null) return NotFound();

            fieldEventItem.IsActive = true;
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Deactivate(int id)
        {
            var fieldEventItem = await _dbContext.FieldEvents.FindAsync(id);
            if (fieldEventItem == null) return NotFound();

            fieldEventItem.IsActive = false;
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

    }



}
