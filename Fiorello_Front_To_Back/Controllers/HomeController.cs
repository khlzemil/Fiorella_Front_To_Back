using Fiorello_Front_To_Back.ViewModels;
using Fiorello_Front_To_Back.ViewModels.Products;
using front_to_back.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fiorello_Front_To_Back.Controllers
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

            var model = new ProductIndexViewModel
            {
                Products = await _appDbContext.Product.Include(x => x.ProductPhotos).
                                               ToListAsync()
            };

            return View(model);
        }
    }
}
