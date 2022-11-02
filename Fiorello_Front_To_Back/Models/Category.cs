namespace Fiorello_Front_To_Back.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public List<Product> Products { get; set; }
    }
}
