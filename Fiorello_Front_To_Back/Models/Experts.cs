using System.ComponentModel.DataAnnotations.Schema;

namespace Fiorello_Front_To_Back.Models
{
    public class Experts
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Position { get; set; }
        public string? PhotoName { get; set; }
        [NotMapped]

        public IFormFile Photo{ get; set; }
    }
}
