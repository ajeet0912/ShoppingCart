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
        [Key]
        public int Id { get; set; }
        [Required]
        public string Code { get; set; }
        public decimal DiscountPercentage { get; set; } // Discount percentage (if applicable)
        public DateTime ExpiryDate { get; set; } // Expiry date of the coupon
        public bool IsActive { get; set; } // Indicates if the coupon is active
        public int FreeItems { get; set; } // Number of free items associated with the coupon (for Type-2)

    }
}
