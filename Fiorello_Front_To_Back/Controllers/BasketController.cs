using Fiorello_Front_To_Back.Models;
using Fiorello_Front_To_Back.ViewModels.Basket;
using front_to_back.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fiorello_Front_To_Back.Controllers
{
    public class BasketController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<User> _userManager;

        public BasketController(AppDbContext appDbContext, UserManager<User> userManager)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var basket = await _appDbContext.Baskets.Include(b => b.BasketProducts).ThenInclude(bp => bp.Product).FirstOrDefaultAsync(b => b.UserId == user.Id);

            var model = new BasketIndexViewModel();

            if (basket == null) return View(model);



            foreach (var dbBasketProduct in basket.BasketProducts)
            {
                var basketProduct = new BasketProductViewModel
                {
                    Id = dbBasketProduct.Id,
                    Title = dbBasketProduct.Product.Title,
                    Quantity = dbBasketProduct.Quantity,
                    PhotoName = dbBasketProduct.Product.PhotoName,
                    StockQuantity = dbBasketProduct.Product.Quantity,
                    Price = dbBasketProduct.Product.Cost,
                };
                model.BasketProducts.Add(basketProduct);
            }
            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> Add(BasketAddViewModel model)
        {
            if (ModelState.IsValid) return BadRequest(model);

            var user = await _userManager.GetUserAsync(User);

            if (user == null) return Unauthorized();
            var product = await _appDbContext.Product.FindAsync(model.Id);

            if (product == null) return NotFound();

            var basket = await _appDbContext.Baskets.FirstOrDefaultAsync(b => b.UserId == user.Id);

            if(basket == null)
            {
                basket = new Basket
                {
                    UserId = user.Id
                };
            }

            await _appDbContext.Baskets.AddAsync(basket);
            await _appDbContext.SaveChangesAsync();
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();
            var basketProduct = await _appDbContext.BasketProducts.FirstOrDefaultAsync(bp => bp.Id == id && bp.Basket.UserId == user.Id);

            if (basketProduct == null) return NotFound();


            var product = await _appDbContext.Product.FirstOrDefaultAsync(p => p.Id == basketProduct.ProductId);

            if (product == null) return NotFound();


            _appDbContext.BasketProducts.Remove(basketProduct);

            await _appDbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]

        public async Task<IActionResult> Down(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();
            var basketProduct = await _appDbContext.BasketProducts.FirstOrDefaultAsync(bp => bp.Id == id && bp.Basket.UserId == user.Id);

            if (basketProduct == null) return NotFound();


            var product = await _appDbContext.Product.FirstOrDefaultAsync(p => p.Id == basketProduct.ProductId);

            if (product == null) return NotFound();


            basketProduct.Quantity--;

            await _appDbContext.SaveChangesAsync();

            return Ok();

        }

        [HttpPost]

        public async Task<IActionResult> Up(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();
            var basketProduct = await _appDbContext.BasketProducts.FirstOrDefaultAsync(bp => bp.Id == id && bp.Basket.UserId == user.Id);

            if (basketProduct == null) return NotFound();


            var product = await _appDbContext.Product.FirstOrDefaultAsync(p => p.Id == basketProduct.ProductId);

            if (product == null) return NotFound();


            basketProduct.Quantity++;

            await _appDbContext.SaveChangesAsync();

            return Ok();

        }

        

    }
}
