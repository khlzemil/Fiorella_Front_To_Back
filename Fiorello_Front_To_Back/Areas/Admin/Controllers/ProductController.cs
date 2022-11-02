using Fiorello_Front_To_Back.Areas.Admin.ViewModels.Products;
using Fiorello_Front_To_Back.Helpers;
using Fiorello_Front_To_Back.Models;
using front_to_back.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Fiorello_Front_To_Back.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(AppDbContext appDbContext, IFileService fileService, IWebHostEnvironment webHostEnvironment)
        {
            _appDbContext = appDbContext;
            _fileService = fileService;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            var model = await _appDbContext.Products.ToListAsync();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {


            var model = new ProductsCreateViewModel
            {
                Categories = await _appDbContext.Categories.Select(c => new SelectListItem
                {
                    Text = c.Title,
                    Value = c.Id.ToString()
                }).ToListAsync()
            };
            return View(model);
        }

        [HttpPost]

        public async Task<IActionResult> Create(ProductsCreateViewModel model)
        {
            model.Categories = await _appDbContext.Categories.Select(c => new SelectListItem
            {
                Text = c.Title,
                Value = c.Id.ToString()
            }).ToListAsync();


            if (!ModelState.IsValid) return View(model);


            var category = await _appDbContext.Categories.FindAsync(model.CategoryId);

            if (category == null)
            {
                ModelState.AddModelError("CategoryId", "Bu kataqoriya movcud deyil");
                return View(model);
            }

            bool isExist = await _appDbContext.Products.AnyAsync(p => p.Title.ToLower().Trim() == model.Title.ToLower().Trim());
            if (isExist)
            {
                ModelState.AddModelError("Title", "Bu adda product movcuddur");
                return View(model);
            }
            if (!_fileService.IsImage(model.MainPhoto))
            {
                ModelState.AddModelError("MainPhoto", "File image formatinda deyil zehmet olmasa image formatinmda secin");
                return View(model);
            }
            if (!_fileService.CheckSize(model.MainPhoto, 400))
            {
                ModelState.AddModelError("MainPhoto", "Image olcusu 400-kbdan boyukdur");
                return View(model);
            }

            bool hasError = false;
            foreach (var photo in model.Photos)
            {
                if (!_fileService.IsImage(photo))
                {
                    ModelState.AddModelError("Photos", $"{photo.FileName} yuklediyiniz file sekil formatinda olmalidir");
                    hasError = true;

                }
                else if (!_fileService.CheckSize(photo, 400))
                {
                    ModelState.AddModelError("Photos", $"{photo.FileName} yuklediyiniz sekil 400-kb dan az olmalidir");
                    hasError = true;

                }

            }

            if (hasError) { return View(model); }


            var product = new Product
            {
                Title = model.Title,
                Cost = model.Cost,
                Description = model.Description,
                Quantity = model.Quantity,
                Weight = model.Weight,
                Dimensions = model.Dimensions,
                CategoryId = model.CategoryId,
                Status = model.Status,
                PhotoName = await _fileService.UploadAsync(model.MainPhoto, _webHostEnvironment.WebRootPath),


            };

            await _appDbContext.Products.AddAsync(product);
            await _appDbContext.SaveChangesAsync();


            int order = 1;
            foreach (var photo in model.Photos)
            {
                var productPhoto = new ProductPhoto
                {
                    PhotoName = await _fileService.UploadAsync(photo, _webHostEnvironment.WebRootPath),
                    Order = order,
                    ProductId = product.Id
                };
                await _appDbContext.ProductPhotos.AddAsync(productPhoto);
                await _appDbContext.SaveChangesAsync();

                order++;
            }

            return RedirectToAction("Index");

        }


        [HttpGet]

        public async Task<IActionResult> Update(int id)
        {
            var product = await _appDbContext.Products.Include(p => p.ProductPhotos).FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            var model = new ProductsUpdateViewModel
            {

                Title = product.Title,
                Cost = product.Cost,
                Description = product.Description,
                Quantity = product.Quantity,
                Weight = product.Weight,
                Dimensions = product.Dimensions,
                CategoryId = product.CategoryId,
                Status = product.Status,
                PhotoName = product.PhotoName,
                ProductPhotos = product.ProductPhotos,



                Categories = await _appDbContext.Categories.Select(c => new SelectListItem
                {
                    Text = c.Title,
                    Value = c.Id.ToString()
                }).ToListAsync()

            };

            return View(model);
        }

        [HttpPost]

        public async Task<IActionResult> Update(ProductsUpdateViewModel model, int id)
        {
            model.Categories = await _appDbContext.Categories.Select(c => new SelectListItem
            {
                Text = c.Title,
                Value = c.Id.ToString()
            }).ToListAsync();

            if (!ModelState.IsValid) return View(model);

            if (id != model.Id) return BadRequest();

            var product = await _appDbContext.Products.Include(p => p.ProductPhotos).FirstOrDefaultAsync(p => p.Id == id);

            model.ProductPhotos = product.ProductPhotos.ToList();



            if (product == null) return NotFound();

            bool isExits = await _appDbContext.Products.AnyAsync(p => p.Title.ToLower().Trim() == product.Title.ToLower().Trim() && p.Id != product.Id);

            if (isExits)
            {
                ModelState.AddModelError("Title", "Bu product artiq movcuddur");
                return View(model);
            }


            product.Title = model.Title;
            product.Cost = model.Cost;
            product.Description = model.Description;
            product.Quantity = model.Quantity;
            product.Weight = model.Weight;
            product.Dimensions = model.Dimensions;
            product.Status = model.Status;

            model.PhotoName = product.PhotoName;

            if (model.MainPhoto != null)
            {

                if (!_fileService.IsImage(model.MainPhoto))
                {
                    ModelState.AddModelError("Photo", "Image formatinda olmalidir");
                    return View(model);
                }
                if (!_fileService.CheckSize(model.MainPhoto, 400))
                {
                    ModelState.AddModelError("Photo", "Sekilin olcusu 400-kb dan boyukdur");
                    return View(model);
                }

                _fileService.Delete(model.PhotoName, _webHostEnvironment.WebRootPath);
                product.PhotoName = await _fileService.UploadAsync(model.MainPhoto, _webHostEnvironment.WebRootPath);
            }

            var category = await _appDbContext.Categories.FindAsync(model.CategoryId);
            if (category == null) return NotFound();
            product.CategoryId = category.Id;


            await _appDbContext.SaveChangesAsync();


            bool hasError = false;

            if (model.Photos != null)
            {
                foreach (var photo in model.Photos)
                {
                    if (!_fileService.IsImage(photo))
                    {
                        ModelState.AddModelError("Photos", $"{photo.FileName} yuklediyiniz file sekil formatinda olmalidir");
                        hasError = true;
                    }
                    else if (!_fileService.CheckSize(photo, 400))
                    {
                        ModelState.AddModelError("Photos", $"{photo.FileName} yuklediyiniz sekil 400 kb dan az olmalidir");
                        hasError = true;
                    }
                }

                if (hasError) { return View(model); }

                int order = 1;
                foreach (var photo in model.Photos)
                {
                    var productPhoto = new ProductPhoto
                    {
                        PhotoName = await _fileService.UploadAsync(photo, _webHostEnvironment.WebRootPath),
                        Order = order,
                        ProductId = product.Id
                    };
                    await _appDbContext.ProductPhotos.AddAsync(productPhoto);
                    await _appDbContext.SaveChangesAsync();

                    order++;
                }
            }


            return RedirectToAction("Index");

        }

        [HttpGet]

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _appDbContext.Products.Include(p => p.ProductPhotos).FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            _fileService.Delete(product.PhotoName, _webHostEnvironment.WebRootPath);

            foreach (var photo in product.ProductPhotos)
            {
                _fileService.Delete(photo.PhotoName, _webHostEnvironment.WebRootPath);

            }
            _appDbContext.Products.Remove(product);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]


        public async Task<IActionResult> UpdatePhoto(int id)
        {

            var productPhoto = await _appDbContext.ProductPhotos.FindAsync(id);
            if (productPhoto == null) return NotFound();

            var model = new ProductsPhotoUpdateViewModel
            {
                Order = productPhoto.Order
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePhoto(int id, ProductsPhotoUpdateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            if (id != model.Id) return BadRequest();

            var productPhoto = await _appDbContext.ProductPhotos.FindAsync(model.Id);
            if (productPhoto == null) return NotFound();

            productPhoto.Order = model.Order;
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("update", "product", new { id = productPhoto.ProductId });


        }
        [HttpGet]

        public async Task<IActionResult> Deletephoto(int id)
        {
            var productPhoto = await _appDbContext.ProductPhotos.FindAsync(id);
            if (productPhoto == null) return NotFound();


            _fileService.Delete(productPhoto.PhotoName, _webHostEnvironment.WebRootPath);

            _appDbContext.ProductPhotos.Remove(productPhoto);
            await _appDbContext.SaveChangesAsync();


            return RedirectToAction("update", "product", new { id = productPhoto.ProductId });
        }
    }
}
