using GamePass.Data;
using GamePass.Models;
using GamePass.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamePass.Repository
{
    public class GenreRepository : RepositoryAsync<Genre>, IGenreRepository
    {
        private readonly ApplicationDbContext _db;

        public GenreRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Genre genre)
        {
            var objDb = _db.Genres.FirstOrDefault(s => s.Id == genre.Id);
            if (objDb != null)
            {
                objDb.Name = genre.Name;
            }

        }
    }
}
