using Fiorello_Front_To_Back.Models;

namespace Fiorello_Front_To_Back.ViewModels.Products
{
    public class ProductDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double Cost { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }

        public string Weight { get; set; }
        public string Dimension { get; set; }

        public Category Category { get; set; }
        public ProductStatus Status { get; set; }
        public string MainPhoto { get; set; }

        public ICollection<ProductPhoto> Photos { get; set; }
    }
}
