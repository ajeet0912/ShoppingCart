using ClothBazar.Data;
using ClothBazar.Entities.Models;
using ClothBazar.Services.Repository.IRepository;
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
    }
}
