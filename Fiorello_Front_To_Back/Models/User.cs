using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Fiorello_Front_To_Back.Models
{
    public class User : IdentityUser
    {
        public string Fullname { get; set; }

        public Basket Basket{ get; set; }
    }
}
