using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface ICRUD<T> where T : class
    {
        Task<IEnumerable<T>> getAll();
        Task<T> getById(Expression<Func<T, bool>> exp);
        Task save(T entity);
        Task update(T entity);
        Task delete(string id);
    }
}
