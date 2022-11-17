namespace Fiorello_Front_To_Back.ViewModels.Basket
{
    public class BasketProductViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public int Quantity { get; set; }
        public int StockQuantity { get; set; }

        public string PhotoName { get; set; }
        public double Price { get; set; }
    }
}
