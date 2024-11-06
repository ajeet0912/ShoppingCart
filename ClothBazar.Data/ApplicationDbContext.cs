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
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<CouponApplication> CouponApplications { get; set; }

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
            new Coupon
            {
                Id = 1,
                Code = "CDP10", // Coupon Code
                DiscountType = (int)DiscountType.AllItemsDiscount, // Type-1: 10% off all items
                DiscountValue = 10, // 10% discount
                ExpiryDate = new DateTime(2025, 12, 31), // Expiry date
                IsActive = true,  // Active coupon
                UsageLimit = 1000, // Can be used up to 1000 times
                TimesUsed = 0 // No uses yet
            },
            new Coupon
            {
                Id = 2,
                Code = "CDPCAP", // Coupon Code
                DiscountType = (int)DiscountType.ProductSpecificDiscount, // Type-2: Buy 2 Jeans, Get 1 Cap Free
                DiscountValue = 0, // Not applicable here, as the discount is based on product pairs
                ExpiryDate = new DateTime(2025, 12, 31), // Expiry date
                IsActive = true,  // Active coupon
                UsageLimit = 500, // Can be used up to 500 times
                TimesUsed = 0 // No uses yet
            }
        );

        }
    }
}