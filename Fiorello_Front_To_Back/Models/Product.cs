namespace Fiorello_Front_To_Back.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string PhotoName { get; set; }
        public string Description { get; set; }
        public double Cost { get; set; }
        public int Quantity { get; set; }
        public string Weight { get; set; }
        public string Dimensions { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public ProductStatus Status { get; set; }

        public List<ProductPhoto> ProductPhotos { get; set; }

        public List<BasketProduct> BasketProducts { get; set; }


        public DateTime CreatedAt { get; set; }

    }

    public enum ProductStatus
    {
        Sold,
        New,
        Sale
    }
}
