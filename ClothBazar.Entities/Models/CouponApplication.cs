using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothBazar.Entities.Models
{
    public class CouponApplication
    {
        public int Id { get; set; }  // Primary key, Identity column
        public int CouponId { get; set; }  // Foreign key to Coupons table
        public int OrderId { get; set; }  // Foreign key to Orders table (you may need to define the Order model if not already present)
        public DateTime AppliedDate { get; set; }  // Date and time when the coupon was applied
        public decimal DiscountApplied { get; set; }  // The discount amount applied to the order
    }

}
