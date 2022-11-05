using Fiorello_Front_To_Back.Areas.Admin.ViewModels.FaqPage;
using Fiorello_Front_To_Back.Models;
using front_to_back.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fiorello_Front_To_Back.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FaqPageController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public FaqPageController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<IActionResult> Index()
        {
            var model = new FaqPageIndexViewModel
            {
                FaqPages = await _appDbContext.FaqPages.ToListAsync()
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(FaqPage faqpage)
        {
            if(!ModelState.IsValid) return View(faqpage);

            bool isExist = await _appDbContext.FaqPages.AnyAsync(rwc => rwc.Title.ToLower().Trim() == faqpage.Title.ToLower().Trim());

            if (isExist)
            {
                ModelState.AddModelError("Title", "Bu adda artıq faq mövcuddur");
                return View(faqpage);
            }

            await _appDbContext.FaqPages.AddAsync(faqpage);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index");

        }

        [HttpGet]

        public async Task<IActionResult> Update(int id)
        {
            var faqPage = await _appDbContext.FaqPages.FindAsync(id);
            if (faqPage == null) return NotFound();
            var model = new FaqPageUpdateViewModel
            {
                Title = faqPage.Title,
                Description = faqPage.Description,
                Order = faqPage.Order
            };

            return View(model);
        }

        [HttpPost]

        public async Task<IActionResult> Update(int id, FaqPageUpdateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            if (id != model.Id) return BadRequest();

            var dbfaqpage = await _appDbContext.FaqPages.FindAsync(id);
            if (dbfaqpage == null) return NotFound();


            bool isExist = await _appDbContext.FaqPages
                                                   .AnyAsync(c => c.Title.ToLower().Trim() == model.Title.
                                                   ToLower().Trim()
                                                   && c.Id != model.Id);

            if (isExist)
            {
                ModelState.AddModelError("Title", "Bu adda title artıq mövcuddur");

                return View(model);
            }

            dbfaqpage.Title = model.Title;
            dbfaqpage.Description = model.Description;
            dbfaqpage.Order = model.Order;
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var faqpage = await _appDbContext.FaqPages.FirstOrDefaultAsync(p => p.Id == id);

            if (faqpage == null) return NotFound();
            var model = new FaqPageDetailsViewModel
            {
                Title = faqpage.Title,
                Description = faqpage.Description,
                Order = faqpage.Order
            };

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _appDbContext.FaqPages.FindAsync(id);

            if (category == null) return NotFound();

             _appDbContext.FaqPages.Remove(category);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("index");
        }
    }
}
