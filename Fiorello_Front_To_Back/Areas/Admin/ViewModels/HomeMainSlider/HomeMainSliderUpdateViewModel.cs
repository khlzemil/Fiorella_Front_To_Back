namespace Fiorello_Front_To_Back.Areas.Admin.ViewModels.HomeMainSlider
{
    public class HomeMainSliderUpdateViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile? SubPhoto { get; set; }
        public string? SubPhotoName { get; set; }
        public List<IFormFile>? Photos { get; set; }
        public ICollection<Models.HomeMainSliderPhoto>? homeMainSliderPhotos { get; set; }
    }
}
