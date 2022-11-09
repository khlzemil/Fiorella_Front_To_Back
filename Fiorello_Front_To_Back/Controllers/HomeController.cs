using Fiorello_Front_To_Back.ViewModels.Home;
using front_to_back.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fiorello_project.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public HomeController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var model = new HomeIndexViewModel
            {
                Products = await _appDbContext.Product.OrderByDescending(p => p.Id).Take(8).ToListAsync(),
                HomeMainSlider = await _appDbContext.HomeMainSliders.Include(hs => hs.HomeMainSliderPhotos.OrderBy(hs => hs.Order)).FirstOrDefaultAsync()
            };

            return View(model);
        }

    }
}