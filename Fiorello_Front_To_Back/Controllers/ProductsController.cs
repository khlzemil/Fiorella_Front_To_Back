﻿using Fiorello_Front_To_Back.ViewModels.Products;
using front_to_back.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fiorello_Front_To_Back.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public ProductsController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<IActionResult> Index()
        {
            var model = new ProductIndexViewModel
            {
                Products = await _appDbContext.Product.OrderByDescending(p => p.Id).ToListAsync()
            };
            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _appDbContext.Product.Include(p => p.ProductPhotos).Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            var model = new ProductDetailsViewModel
            {
                Title = product.Title,
                Cost = product.Cost,
                Description = product.Description,
                Quantity = product.Quantity,
                Weight = product.Weight,
                Dimension = product.Dimensions,
                Category = product.Category,
                Status = product.Status,
                MainPhoto = product.PhotoName,
                Photos = product.ProductPhotos
            };

            return View(model);
        }
    }
}
