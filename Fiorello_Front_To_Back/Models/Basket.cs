namespace Fiorello_Front_To_Back.Models
{
    public class Basket
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public User user{ get; set; }
        public ICollection<BasketProduct> BasketProducts { get; set; }
    }
}
