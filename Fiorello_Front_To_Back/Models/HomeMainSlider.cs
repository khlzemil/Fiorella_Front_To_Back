namespace Fiorello_Front_To_Back.Models
{
    public class HomeMainSlider
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string SubPhotoName { get; set; }
        public List<HomeMainSliderPhoto>? HomeMainSliderPhotos { get; set; }
    }
}
