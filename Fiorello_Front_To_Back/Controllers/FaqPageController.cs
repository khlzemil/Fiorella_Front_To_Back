using Fiorello_Front_To_Back.ViewModels.FaqPage;
using front_to_back.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fiorello_Front_To_Back.Controllers
{
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
                FaqPages = await _appDbContext.FaqPages.OrderBy(c => c.Order).ToListAsync()
            };
            return View(model);

        }
    }
}
