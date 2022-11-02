using Microsoft.AspNetCore.Mvc;

namespace Fiorello_Front_To_Back.Controllers
{
    public class FaqPageController : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
