using Fiorello_Front_To_Back.Areas.Admin.ViewModels.HomeMainSlider;
using Fiorello_Front_To_Back.Helpers;
using Fiorello_Front_To_Back.Models;
using front_to_back.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fiorello_Front_To_Back.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeMainSliderController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFileService _fileService;

        public HomeMainSliderController(AppDbContext appDbContext, IWebHostEnvironment webHostEnvironment, IFileService fileService)
        {
            _appDbContext = appDbContext;
            _webHostEnvironment = webHostEnvironment;
            _fileService = fileService;
        }
        public async Task<IActionResult> Index()
        {
            var model = new HomeMainSliderIndexViewModel
            {
                HomeMainSlider = await _appDbContext.HomeMainSliders.FirstOrDefaultAsync()
            };
            return View(model);

        }

        [HttpGet]

        public async Task<IActionResult> Create()
        {
            var homeMainSlider = await _appDbContext.HomeMainSliders.FirstOrDefaultAsync();
            if (homeMainSlider != null) return NotFound();
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Create(HomeMainSliderCreateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            bool isExist = await _appDbContext.HomeMainSliders.AnyAsync(hs => hs.Title.ToLower().Trim() == model.Title.ToLower().Trim());
            if (isExist)
            {
                ModelState.AddModelError("Title", "Bu adda Slider movcuddur");
                return View(model);
            }

            if (!_fileService.IsImage(model.SubPhoto))
            {
                ModelState.AddModelError("SubPhoto", "File image formatinda deyil zehmet olmasa image formasinda secin!!");
                return View(model);
            }
            if (!_fileService.CheckSize(model.SubPhoto, 300))
            {
                ModelState.AddModelError("SubPhoto", "File olcusu 300 kbdan boyukdur");
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
                else if (!_fileService.CheckSize(photo, 300))
                {
                    ModelState.AddModelError("Photos", $"{photo.FileName} ci yuklediyiniz sekil 300 kb dan az olmalidir");
                    hasError = true;

                }

            }

            if (hasError) return View(model);

            var homeMainSlider = new HomeMainSlider
            {
                Title = model.Title,
                Description = model.Description,
                SubPhotoName = await _fileService.UploadAsync(model.SubPhoto, _webHostEnvironment.WebRootPath),
            };


            await _appDbContext.HomeMainSliders.AddAsync(homeMainSlider);
            await _appDbContext.SaveChangesAsync();

            int order = 1;
            foreach (var photo in model.Photos)
            {
                var homeMainSliderPhoto = new HomeMainSliderPhoto
                {
                    Name = await _fileService.UploadAsync(photo, _webHostEnvironment.WebRootPath),
                    Order = order,
                    HomeMainSliderId = homeMainSlider.Id
                };
                await _appDbContext.HomeMainSliderPhotos.AddAsync(homeMainSliderPhoto);
                await _appDbContext.SaveChangesAsync();

                order++;
            }

            return RedirectToAction("Index");
        }


        [HttpGet]

        public async Task<IActionResult> Delete(int id)
        {
            var homeMainSlider = await _appDbContext.HomeMainSliders.Include(hs => hs.HomeMainSliderPhotos).FirstOrDefaultAsync(hs => hs.Id == id);

            if (homeMainSlider == null) return NotFound();

            _fileService.Delete(homeMainSlider.SubPhotoName, _webHostEnvironment.WebRootPath);

            foreach (var photo in homeMainSlider.HomeMainSliderPhotos)
            {
                _fileService.Delete(photo.Name, _webHostEnvironment.WebRootPath);

            }
            _appDbContext.HomeMainSliders.Remove(homeMainSlider);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        [HttpGet]

        public async Task<IActionResult> Details(int id)
        {
            var homeMainSlider = await _appDbContext.HomeMainSliders.Include(hs => hs.HomeMainSliderPhotos).FirstOrDefaultAsync(hs => hs.Id == id);



            if (homeMainSlider == null) return NotFound();

            var model = new HomeMainSliderDetailsViewModel
            {
                Id = homeMainSlider.Id,
                Title = homeMainSlider.Title,

                Description = homeMainSlider.Description,
                SubPhotoName = homeMainSlider.SubPhotoName,
                Photos = homeMainSlider.HomeMainSliderPhotos,
            };

            return View(model);
        }


        [HttpGet]

        public async Task<IActionResult> Update(int id)
        {
            var homeMainSlider = await _appDbContext.HomeMainSliders.Include(hs => hs.HomeMainSliderPhotos).FirstOrDefaultAsync(hs => hs.Id == id);



            if (homeMainSlider == null) return NotFound();

            var model = new HomeMainSliderUpdateViewModel
            {
                Title = homeMainSlider.Title,
                Description = homeMainSlider.Description,
                SubPhotoName = homeMainSlider.SubPhotoName,
                homeMainSliderPhotos = homeMainSlider.HomeMainSliderPhotos,
            };

            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> Update(HomeMainSliderUpdateViewModel model, int id)
        {

            if (!ModelState.IsValid) return View(model);

            if (id != model.Id) return BadRequest();

            var homeMainSlider = await _appDbContext.HomeMainSliders.Include(hs => hs.HomeMainSliderPhotos).FirstOrDefaultAsync(hs => hs.Id == id);

            model.homeMainSliderPhotos = homeMainSlider.HomeMainSliderPhotos.ToList();


            if (homeMainSlider == null) return NotFound();

            if (model.SubPhoto != null)
            {

                if (!_fileService.IsImage(model.SubPhoto))
                {
                    ModelState.AddModelError("Photo", "Image formatinda secin");
                    return View(model);
                }
                if (!_fileService.CheckSize(model.SubPhoto, 300))
                {
                    ModelState.AddModelError("Photo", "Sekilin olcusu 300 kb dan boyukdur");
                    return View(model);
                }

                _fileService.Delete(homeMainSlider.SubPhotoName, _webHostEnvironment.WebRootPath);
                homeMainSlider.SubPhotoName = await _fileService.UploadAsync(model.SubPhoto, _webHostEnvironment.WebRootPath);
            }







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
                    else if (!_fileService.CheckSize(photo, 300))
                    {
                        ModelState.AddModelError("Photos", $"{photo.FileName} ci yuklediyiniz sekil 300 kb dan az olmalidir");
                        hasError = true;
                    }
                }

                if (hasError) { return View(model); }
                var homeMainSliderPhoto = homeMainSlider.HomeMainSliderPhotos.OrderByDescending(hs => hs.Order).FirstOrDefault();
                int order = homeMainSliderPhoto != null ? homeMainSliderPhoto.Order : 0;
                foreach (var photo in model.Photos)
                {
                    var productPhoto = new HomeMainSliderPhoto
                    {
                        Name = await _fileService.UploadAsync(photo, _webHostEnvironment.WebRootPath),
                        Order = ++order,
                        HomeMainSliderId = homeMainSlider.Id
                    };
                    await _appDbContext.HomeMainSliderPhotos.AddAsync(productPhoto);
                    await _appDbContext.SaveChangesAsync();


                }
            }
            homeMainSlider.Title = model.Title;

            homeMainSlider.Description = model.Description;


            model.SubPhotoName = homeMainSlider.SubPhotoName;



            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> UpdatePhoto(int id)
        {

            var homeMainSliderPhoto = await _appDbContext.HomeMainSliderPhotos.FindAsync(id);
            if (homeMainSliderPhoto == null) return NotFound();

            var model = new HomeMainSliderPhotoUpdateViewModel
            {
                Order = homeMainSliderPhoto.Order
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePhoto(int id, HomeMainSliderPhotoUpdateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            if (id != model.Id) return BadRequest();

            var homeMainSliderPhoto = await _appDbContext.HomeMainSliderPhotos.FindAsync(model.Id);
            if (homeMainSliderPhoto == null) return NotFound();

            homeMainSliderPhoto.Order = model.Order;
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("update", "homemainslider", new { id = homeMainSliderPhoto.HomeMainSliderId });


        }

        [HttpGet]

        public async Task<IActionResult> Deletephoto(int id)
        {
            var homeMainSliderPhoto = await _appDbContext.HomeMainSliderPhotos.FindAsync(id);
            if (homeMainSliderPhoto == null) return NotFound();


            _fileService.Delete(homeMainSliderPhoto.Name, _webHostEnvironment.WebRootPath);

            _appDbContext.HomeMainSliderPhotos.Remove(homeMainSliderPhoto);
            await _appDbContext.SaveChangesAsync();


            return RedirectToAction("update", "homemainslider", new { id = homeMainSliderPhoto.HomeMainSliderId });
        }
    }
}
