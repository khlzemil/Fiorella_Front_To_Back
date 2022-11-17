using Fiorello_Front_To_Back.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Fiorello_Front_To_Back.Areas.Admin.ViewModels.Products
{
    public class ProductsIndexViewModel
    {
        public List<Product> Products { get; set; }
        #region Filter
        public List<SelectListItem> Categories{ get; set; }
        public string? Title { get; set; }
        [Display(Name = "Minimum Price")]
        public double? MinPrice { get; set; }
        [Display(Name = "Max Price")]
        public double? MaxPrice { get; set; }
        [Display(Name = "Min Quantity")]
        public int? MinQuantity{ get; set; }
        [Display(Name = "Max Quantity")]
        public int? MaxQuantity{ get; set; }
        [Display(Name = "Careted Start Date")]
        public DateTime? CreatedAtStart{ get; set; }
        [Display(Name = "Careted Start Date")]
        public DateTime? CreatedAtEnd { get; set; }
        [Display(Name = "Status")]
        public ProductStatus? Status { get; set; }
        [Display(Name = "Category")]
        public int?  CategoryId{ get; set; }
        #endregion
    }
}
