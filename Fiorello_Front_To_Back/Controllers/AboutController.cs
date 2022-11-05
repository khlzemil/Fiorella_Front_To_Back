using Fiorello_Front_To_Back.ViewModels.About;
using front_to_back.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fiorello_Front_To_Back.Controllers
{
    public class AboutController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public AboutController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<IActionResult> Index()
        {
            var model = new ExpertIndexViewModel
            {
                Experts = await _appDbContext.Experts.ToListAsync()
            };
            return View(model);
        }
    }
}
