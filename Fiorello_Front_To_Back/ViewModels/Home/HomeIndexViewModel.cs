namespace Fiorello_Front_To_Back.ViewModels.Home
{
    public class HomeIndexViewModel
    {
        public List<Models.Experts> Experts { get; set; }
        public List<Models.Product> Products { get; set; }

        public Models.HomeMainSlider HomeMainSlider { get; set; }
    }
}
