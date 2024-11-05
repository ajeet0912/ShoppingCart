using ClothBazar.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothBazar.Entities.ViewModels
{
    public class ShoppingCartViewModels
    {
        public IEnumerable<ShoppingCart> ListShoppingCart { get; set; }

        public decimal OrderTotal { get; set; }
    }
}
