using Microsoft.AspNetCore.Mvc;

namespace Fiorello_Front_To_Back.Controllers
{
    public class BlogsController : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> Details()
        {
            return View();
        }
    }
}
