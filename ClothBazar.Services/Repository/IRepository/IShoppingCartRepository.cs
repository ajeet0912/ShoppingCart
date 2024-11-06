using ClothBazar.Entities.Models;
using ClothBazar.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothBazar.Services.Repository.IRepository
{
    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    {
        Task<ShoppingCart> UpdateAsync(ShoppingCart shoppingCart);

        Task<decimal> ApplyCouponAsync(string couponCode, ShoppingCartViewModels model);
        Task<string> ValidateCouponAsync(string couponCode);
        Task<bool> IsCouponValidAsync(string couponCode);
    }
}
