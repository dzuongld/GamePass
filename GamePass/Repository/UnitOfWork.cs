using GamePass.Data;
using GamePass.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamePass.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public IGenreRepository Genre { get; private set; }
        public IPlatformRepository Platform { get; private set; }
        public IProductRepository Product { get; private set; }

        public ISP_Call SP_Call { get; private set; }

        public IApplicationUserRepository ApplicationUser { get; private set; }

        public IOrderDetailsRepository OrderDetails { get; private set; }

        public IOrderHeaderRepository OrderHeader { get; private set; }

        public IShoppingCartRepository ShoppingCart { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Genre = new GenreRepository(_db);
            Platform = new PlatformRepository(_db);
            Product = new ProductRepository(_db);
            ApplicationUser = new ApplicationUserRepository(_db);
            OrderDetails = new OrderDetailsRepository(_db);
            OrderHeader = new OrderHeaderRepository(_db);
            ShoppingCart = new ShoppingCartRepository(_db);
            SP_Call = new SP_Call(_db);
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
