using Fiorello_Front_To_Back.ViewModels.Basket;
using front_to_back.DAL;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Fiorello_Front_To_Back.Controllers
{
    public class BasketController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public BasketController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<IActionResult> Index()
        {

            var basketProducts = JsonConvert.DeserializeObject<List<BasketAddViewModel>>(Request.Cookies["basket"]);

            List<BasketListItemViewModel> model = new List<BasketListItemViewModel>();

            foreach(var basketProduct in basketProducts)
            {
                var dbproduct = await _appDbContext.Product.FindAsync(basketProduct.Id);
                if(dbproduct != null)
                {
                    model.Add(new BasketListItemViewModel
                    {
                        Id = dbproduct.Id,
                        Title = dbproduct.Title,
                        Price = dbproduct.Cost,
                        Quantity = basketProduct.Count,
                        StockQuantity = dbproduct.Quantity,
                        PhotoName = dbproduct.PhotoName,
                    });
                }
            }
            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> Add(BasketAddViewModel model)
        {
            List<BasketAddViewModel> basket;
            if (Request.Cookies["basket"] != null)
            {
                basket = JsonConvert.DeserializeObject<List<BasketAddViewModel>>(Request.Cookies["basket"]);
            }
            else
            {
                basket = new List<BasketAddViewModel>();
            }
            var basketProduct = basket.Find(b => b.Id == model.Id);

            if(basketProduct != null)
            {
                basketProduct.Count++;
            }
            else
            {
                model.Count++;
                basket.Add(model);
            }

            var serializedBasket = JsonConvert.SerializeObject(basket);

            Response.Cookies.Append("basket", serializedBasket);
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {

            List<BasketAddViewModel> basket;
            if (Request.Cookies["basket"] == null) return NotFound();

            basket = JsonConvert.DeserializeObject<List<BasketAddViewModel>>(Request.Cookies["basket"]);

            var dbProduct = await _appDbContext.Product.FindAsync(id);
            if (dbProduct == null) return NotFound();

            var basketProduct = basket.Find(b => b.Id == dbProduct.Id);
            if (basketProduct != null)
            {
                basket.Remove(basketProduct);
            }
            var serializedBasket = JsonConvert.SerializeObject(basket);
            Response.Cookies.Append("basket", serializedBasket);

            return Ok();

        }


    }
}
