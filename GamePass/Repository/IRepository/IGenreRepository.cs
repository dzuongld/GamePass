using GamePass.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamePass.Repository.IRepository
{
    public interface IGenreRepository : IRepositoryAsync<Genre>
    {
        void Update(Genre genre);
    }
}
