using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothBazar.Entities.Models
{
    public class Coupon
    {
        public int Id { get; set; }  // Primary key, Identity column
        public string Code { get; set; }  // Coupon code (unique)
        public int DiscountType { get; set; }  // Type of discount (e.g., 1 for percentage, 2 for product-specific)
        public decimal DiscountValue { get; set; }  // Discount amount or percentage
        public DateTime ExpiryDate { get; set; }  // Expiry date of the coupon
        public bool IsActive { get; set; }  // Whether the coupon is active or not
        public int UsageLimit { get; set; }  // Maximum usage limit for the coupon
        public int TimesUsed { get; set; }  // Times this coupon has been used

    }

    public enum DiscountType
    {
        AllItemsDiscount = 1, // Discount on all items (e.g., 10% off)
        ProductSpecificDiscount = 2, // Discount on specific items (e.g., Buy 2 Jeans, get 1 Cap free)
                                     // Add more discount types as needed
    }
}
