using GamePass.Data;
using GamePass.Models;
using GamePass.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamePass.Repository
{
    public class PlatformRepository : Repository<Platform>, IPlatformRepository
    {
        private readonly ApplicationDbContext _db;

        public PlatformRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Platform platform)
        {
            var objDb = _db.Platforms.FirstOrDefault(s => s.Id == platform.Id);
            if (objDb != null)
            {
                objDb.Name = platform.Name;
            }

        }
    }
}
