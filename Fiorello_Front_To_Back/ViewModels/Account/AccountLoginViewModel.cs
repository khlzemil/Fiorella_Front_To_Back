using System.ComponentModel.DataAnnotations;

namespace Fiorello_Front_To_Back.ViewModels.MyAccount
{
    public class AccountLoginViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required, MaxLength(100), DataType(DataType.Password)]
        public string Password { get; set; }

        public string? ReturnUrl { get; set; }
    }
}
