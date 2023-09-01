using Microsoft.AspNetCore.Mvc;
using VertoTask.Data;
using VertoTask.Models;
using Microsoft.EntityFrameworkCore;

namespace VertoTask.Controllers
{
    public class AdminProductController : Controller
    {
        private readonly ApplicationDBContext _dbContext;
        private const int MaxImageSize = 5 * 1024 * 1024;
        private const string UploadsFolder = "wwwroot/images/uploads";
        public AdminProductController(ApplicationDBContext dBContext)
        {
            _dbContext = dBContext;
            
        }
        public IActionResult Index()
        {
            var products = _dbContext.Products.ToList();
            return View(products);
        }

        public IActionResult Create() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product, IFormFile imageFile)
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

                    

                    product.ImageUrl = "/images/uploads/" + uniqueFileName;
                }
                else
                {
                    
                    ModelState.AddModelError("ImageError", "Invalid image size or type.");
                    return View(product);
                }
            }

            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index", "Home");

        }




        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = _dbContext.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }


        [HttpPost, ActionName("Edit")]
        public async Task<IActionResult> Edit(int id, Product product, IFormFile newImageFile)
        {
            if (id != product.Id)
            {
                return NotFound();
            }


            var productEdit = await _dbContext.Products.FindAsync(product.Id);
            if(productEdit != null)
            {
                productEdit.Id = product.Id;
                productEdit.ProductName = product.ProductName;

                productEdit.Description = product.Description;
                productEdit.Price = product.Price;

                if (newImageFile != null && newImageFile.Length > 0)
                {
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(newImageFile.FileName);
                    var imagePath = Path.Combine(UploadsFolder, uniqueFileName);
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await newImageFile.CopyToAsync(stream);
                    }


                    productEdit.ImageUrl = "/images/uploads/" + uniqueFileName;
                }

                await _dbContext.SaveChangesAsync();
                return RedirectToAction("Index");

            }
            return RedirectToAction("Index");
}
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var product = _dbContext.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> Delete(int id, Product pdt)
        {
            var product = _dbContext.Products.Find(id);
            if (product != null)
            {
                _dbContext.Products.Remove(product);
                await _dbContext.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ActivateForOfferSlider(int id)
        {
            var productItem = await _dbContext.Products.FindAsync(id);
            if (productItem == null) return NotFound();

            productItem.IsForProductOfferSlider = true;
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeactivateForOfferSlider(int id)
        {
            var productItem = await _dbContext.Products.FindAsync(id);
            if (productItem == null) return NotFound();

            productItem.IsForProductOfferSlider = false;
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> ActivateForSingleProduct(int id)
        {
            var productItem = await _dbContext.Products.FindAsync(id);
            if (productItem == null) return NotFound();

            productItem.IsForSingleProductDisplay = true;
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeactivateForSingleProduct(int id)
        {
            var productItem = await _dbContext.Products.FindAsync(id);
            if (productItem == null) return NotFound();

            productItem.IsForSingleProductDisplay = false;
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

    }


}
