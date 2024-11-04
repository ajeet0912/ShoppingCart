using ClothBazar.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace ClothBazar.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "T-shirt", Price = 2000, Description = "This is T-shirt" },
                new Product { Id = 2, Name = "Jeans", Price = 4000, Description = "This is Jeans" },
                new Product { Id = 3, Name = "Shoes", Price = 6000, Description = "This is Shoes" },
                new Product { Id = 4, Name = "Caps", Price = 2000, Description = "This is Caps" }
            );

            builder.Entity<Coupon>().HasData(
                new Coupon { Id = 1, Code = "CDP10", DiscountPercentage = 10, ExpiryDate = DateTime.Now.AddDays(10), IsActive = true, FreeItems = 0 },
                new Coupon { Id = 2, Code = "CDPCAP", DiscountPercentage = 10, ExpiryDate = DateTime.Now.AddDays(10), IsActive = true, FreeItems = 0 }
            );

        }
    }
}