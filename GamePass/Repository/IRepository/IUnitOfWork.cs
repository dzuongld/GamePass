using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamePass.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IGenreRepository Genre { get; }
        IPlatformRepository Platform { get; }
        IProductRepository Product { get; }
        IApplicationUserRepository ApplicationUser { get; }
        ISP_Call SP_Call { get; }
        IOrderDetailsRepository OrderDetails { get; }
        IOrderHeaderRepository OrderHeader { get; }
        IShoppingCartRepository ShoppingCart { get; }
        void Save();
    }
}
