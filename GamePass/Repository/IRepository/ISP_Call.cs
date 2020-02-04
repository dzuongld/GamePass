using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamePass.Repository.IRepository
{
    public interface ISP_Call : IDisposable
    {
        // a piece of 1 record
        T Single<T>(string procedureName, DynamicParameters param = null);

        void Execute(string procedureName, DynamicParameters param = null);

        // full record
        T OneRecord<T>(string procedureName, DynamicParameters param = null);

        // all rows
        IEnumerable<T> List<T>(string procedureName, DynamicParameters param = null);

        // involve 2 tables
        Tuple<IEnumerable<T1>, IEnumerable<T2>> List<T1, T2>(string procedureName, DynamicParameters param = null);
    }
}
