using Fiorello_Front_To_Back.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Fiorello_Front_To_Back.Areas.Admin.ViewModels.Products
{
    public class ProductsDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double Cost { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }

        public string Weight { get; set; }
        public string Dimensions { get; set; }

        public int CategoryId { get; set; }
        public List<SelectListItem>? Categories { get; set; }
        public ProductStatus Status { get; set; }

        public string PhotoName { get; set; }


        public ICollection<Models.ProductPhoto> Photos { get; set; }
    }
}
