using Fiorello_Front_To_Back.Areas.Admin.ViewModels.Experts;
using Fiorello_Front_To_Back.Helpers;
using Fiorello_Front_To_Back.Models;
using front_to_back.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fiorello_Front_To_Back.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ExpertsController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFileService _fileService;

        public ExpertsController(AppDbContext appDbContext, IWebHostEnvironment webHostEnvironment, IFileService fileService)
        {
            _appDbContext = appDbContext;
            _webHostEnvironment = webHostEnvironment;
            _fileService = fileService;
        }
        public async Task<IActionResult> Index()
        {
            var model = new ExpertsIndexViewModel
            {
               Experts = await _appDbContext.Experts.ToListAsync()
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Experts experts)
        {
            if (!ModelState.IsValid) return View(experts);

            if (!_fileService.IsImage(experts.Photo))
            {
                ModelState.AddModelError("Photo", "File must be image format");
                return View(experts);
            }
            if (!_fileService.CheckSize(experts.Photo, 400))
            {
                ModelState.AddModelError("Photo", "Image must be less than 400 kb");
                return View(experts);
            }
            experts.PhotoName = await _fileService.UploadAsync(experts.Photo, _webHostEnvironment.WebRootPath);
            await _appDbContext.Experts.AddAsync(experts);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("index");

        }

        [HttpGet]
        public async Task<IActionResult> Update()
        {
            var expert = await _appDbContext.Experts.FirstOrDefaultAsync();

            if (expert == null) return NotFound();

            var model = new ExpertsUpdateViewModel
            {
                Name = expert.Name,
                Surname = expert.Surname,
                Position = expert.Position,
                Photo = expert.Photo
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ExpertsUpdateViewModel model)
        {
            var expert = await _appDbContext.Experts.FirstOrDefaultAsync();

            if (expert == null) return NotFound();

            if (!ModelState.IsValid) return View(model);

            expert.Name = model.Name;
            expert.Surname = model.Surname;
            expert.Position = model.Position;

            await _appDbContext.SaveChangesAsync();


            if (model.Photo != null)
            {
                if (!_fileService.IsImage(model.Photo))
                {
                    ModelState.AddModelError("Photo", "File must be image format");
                    return View(model);
                }
                else if (!_fileService.CheckSize(model.Photo, 400))
                {
                    ModelState.AddModelError("Photo", "Image must be less than 400 kb");
                    return View(model);
                }
                _fileService.Delete(_webHostEnvironment.WebRootPath, expert.PhotoName);
                expert.PhotoName = await _fileService.UploadAsync(model.Photo, _webHostEnvironment.WebRootPath);
            }
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("index");

        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var expert = await _appDbContext.Experts.FindAsync(id);
            if (expert == null) return NotFound();

            var model = new ExpertsDetailsViewModel
            {
                Id = expert.Id,
                Name = expert.Name,
                Surname = expert.Surname,
                Position = expert.Position,
                PhotoName = expert.PhotoName
            };
            return View(model);
        }


        [HttpGet]

        public async Task<IActionResult> Delete(int id)
        {
            var expert = await _appDbContext.Experts.FindAsync(id);

            if (expert == null) return NotFound();

            return View(expert);
        }

        [HttpPost]

        public async Task<IActionResult> DeleteComponent(int id)
        {
            var expert = await _appDbContext.Experts.FindAsync(id);

            if (expert == null) return NotFound();

            return View(expert);
        }
    }
}
