namespace Fiorello_Front_To_Back.Models
{
    public class ProductPhoto
    {
        public int Id { get; set; }
        public string PhotoName { get; set; }
        public int Order { get; set; }
        public int ProductId{ get; set; }
        public Product Product { get; set; }
    }
}
