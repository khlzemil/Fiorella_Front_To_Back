namespace Fiorello_Front_To_Back.Areas.Admin.ViewModels.Experts
{
    public class ExpertsDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Position { get; set; }
        public string PhotoName { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
