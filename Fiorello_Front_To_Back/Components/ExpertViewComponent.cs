using Microsoft.AspNetCore.Mvc;
using front_to_back.DAL;
using Microsoft.EntityFrameworkCore;
using Fiorello_Front_To_Back.ViewModels.About;

namespace Fiorello_Front_To_Back.Components
{
    public class ExpertViewComponent : ViewComponent
    {
        private readonly AppDbContext _appDbContext;

        public ExpertViewComponent(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new ExpertIndexViewModel
            {
                Experts = await _appDbContext.Experts.ToListAsync()
            };

            return View(model);
        }

    }
}
