using ClothBazar.Entities.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothBazar.Entities.ViewModels
{
    public class ShoppingCartViewModels
    {
        [ValidateNever]
        public IEnumerable<ShoppingCart> ListShoppingCart { get; set; }

        public decimal OrderTotal { get; set; }

        [ValidateNever]
        public Coupon Coupon { get; set; }
    }
}
