using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GamePass.Repository.IRepository
{
    // make it generic
    public interface IRepository<T> where T : class
    {
        T Get(int id);

        // add options 
        IEnumerable<T> GetAll(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = null
            );

        T GetFirstOrDefault(
            Expression<Func<T, bool>> filter = null,
            string includeProperties = null
            );

        void Add(T entity);

        void Remove(int id);

        void Remove(T entity);

        void RemoveRange(IEnumerable<T> entity);
    }
}
