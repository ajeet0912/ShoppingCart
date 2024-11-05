using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothBazar.Services.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IProductRepository ProductRepository { get; }
        IShoppingCartRepository ShoppingCartRepository { get; }
        IApplicationUserRepository ApplicationUserRepository { get; }
        Task SaveAsync();
    }
}
