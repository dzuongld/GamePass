using GamePass.Data;
using GamePass.Models;
using GamePass.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamePass.Repository
{
    public class GenreRepository : Repository<Genre>, IGenreRepository
    {
        private readonly ApplicationDbContext _db;

        public GenreRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Genre category)
        {
            var objDb = _db.Genres.FirstOrDefault(s => s.Id == category.Id);
            if (objDb != null)
            {
                objDb.Name = category.Name;
                _db.SaveChanges();
            }

        }
    }
}
