using GamePass.Data;
using GamePass.Models;
using GamePass.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamePass.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Product product)
        {
            var objDb = _db.Products.FirstOrDefault(s => s.Id == product.Id);
            if (objDb != null)
            {
                objDb.Title = product.Title;
                objDb.Price = product.Price;
                objDb.Description = product.Description;
                objDb.GenreId = product.GenreId;
                objDb.Publisher = product.Publisher;
                objDb.PlatformId = product.PlatformId;
            }

        }
    }
}
