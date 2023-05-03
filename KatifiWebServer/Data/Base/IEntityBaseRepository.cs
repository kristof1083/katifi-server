using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace KatifiWebServer.Data.Base
{
    public interface IEntityBaseRepository<T> where T : class, IEntityBase, new()
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties);

        Task<T> GetByIdAsync(int id);

        Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] includeProperties);

        Task AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(int id);

        Task<bool> EntityExists(int id);

        bool MeetsTheConstraints(T entity);

    }
}
