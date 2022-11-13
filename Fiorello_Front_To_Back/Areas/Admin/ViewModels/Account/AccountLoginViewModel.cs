using System.ComponentModel.DataAnnotations;

namespace Fiorello_Front_To_Back.Areas.Admin.ViewModels.Account
{
    public class AccountLoginViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
