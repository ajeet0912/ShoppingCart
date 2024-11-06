using ClothBazar.Data;
using ClothBazar.Entities.Models;
using ClothBazar.Entities.ViewModels;
using ClothBazar.Services.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothBazar.Services.Repository
{
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        private ApplicationDbContext _db;
        public ShoppingCartRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<ShoppingCart> UpdateAsync(ShoppingCart shoppingCart)
        {
            _db.Update(shoppingCart);
            return shoppingCart;
        }

        public async Task<decimal> ApplyCouponAsync(string couponCode, ShoppingCartViewModels model)
        {
            var coupon = await _db.Coupons
                .FirstOrDefaultAsync(c => c.Code == couponCode && c.IsActive && c.ExpiryDate >= DateTime.Now);

            if (coupon == null) return 0;

            decimal discountAmount = coupon.DiscountType switch
            {
                (int)DiscountType.AllItemsDiscount => model.OrderTotal * (coupon.DiscountValue / 100),
                (int)DiscountType.ProductSpecificDiscount => await ApplyProductSpecificDiscountAsync(model),
                _ => throw new InvalidOperationException("Invalid coupon type.")
            };

            await LogCouponUsageAsync(coupon.Id, model.ListShoppingCart.FirstOrDefault().Id, discountAmount);
            return discountAmount;
        }

        public async Task<string> ValidateCouponAsync(string couponCode)
        {
            var coupon = await _db.Coupons.FirstOrDefaultAsync(c => c.Code == couponCode);
            if (coupon == null) return "Invalid Coupon Code.";
            if (coupon.ExpiryDate < DateTime.Now) return "Coupon Expired.";
            if (!coupon.IsActive) return "Coupon is not active.";
            return "Coupon is valid.";
        }

        private async Task<decimal> ApplyProductSpecificDiscountAsync(ShoppingCartViewModels model)
        {
            decimal discountAmount = 0;
            int jeansCount = model.ListShoppingCart.Count(item => item.Product.Name.Contains("Jeans"));
            int freeCaps = jeansCount / 2;

            foreach (var item in model.ListShoppingCart)
            {
                if (item.Product.Name.Contains("Cap") && freeCaps > 0)
                {
                    discountAmount += item.Product.Price;
                    freeCaps--;
                }
            }

            return discountAmount;
        }

        private async Task LogCouponUsageAsync(int couponId, int orderId, decimal discountApplied)
        {
            var couponApplication = new CouponApplication
            {
                CouponId = couponId,
                OrderId = orderId,
                AppliedDate = DateTime.Now,
                DiscountApplied = discountApplied
            };

            _db.CouponApplications.Add(couponApplication);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> IsCouponValidAsync(string couponCode)
        {
            // Check if the coupon exists
            var coupon = await _db.Coupons
                .FirstOrDefaultAsync(c => c.Code == couponCode && c.IsActive);

            // If the coupon doesn't exist or is inactive, return false
            if (coupon == null)
            {
                return false; // Coupon doesn't exist or is inactive
            }

            // Check if the coupon has expired
            if (coupon.ExpiryDate < DateTime.Now)
            {
                return false; // Coupon is expired
            }

            // Check if the coupon usage limit has been reached
            if (coupon.TimesUsed >= coupon.UsageLimit)
            {
                return false; // Coupon usage limit reached
            }

            // If all checks pass, the coupon is valid
            return true;
        }
    }
}

