using Fiorello_Front_To_Back.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace front_to_back.DAL
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductPhoto> ProductPhotos { get; set;}
        public DbSet<Experts> Experts{ get; set; }
        public DbSet<FaqPage> FaqPages { get; set; }
        public DbSet<HomeMainSlider> HomeMainSliders { get; set; }

        public DbSet<HomeMainSliderPhoto> HomeMainSliderPhotos { get; set; }
        public DbSet<Blog> Blogs { get; set; }

        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketProduct> BasketProducts{ get; set; }

    }
}
