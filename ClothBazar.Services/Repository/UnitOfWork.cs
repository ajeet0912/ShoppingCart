using ClothBazar.Data;
using ClothBazar.Services.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothBazar.Services.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public IProductRepository ProductRepository { get; private set; }
        public IShoppingCartRepository ShoppingCartRepository { get; private set; }
        public IApplicationUserRepository ApplicationUserRepository { get; private set; }
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            ProductRepository = new ProductRepository(_db);
            ShoppingCartRepository = new ShoppingCartRepository(_db);
            ApplicationUserRepository = new ApplicationUserRepository(_db);
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
