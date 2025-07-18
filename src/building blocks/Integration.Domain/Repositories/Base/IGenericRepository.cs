using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Integration.Domain.Repositories.Base
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(int? skip = null, int? take = null);

        Task<T> GetByIdAsync(Guid id);

        Task<T> GetByIdAsync(int id);

        Task<T> GetDataAsync(
            Expression<Func<T, bool>> expression = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);

        Task<IEnumerable<T>> GetListDataAsync(
            Expression<Func<T, bool>> expression = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            int? skip = null, int? take = null);

        Task AddAsync(T entity);

        Task UpdateAsync(T entity, bool modifySingleEntity = false);

        void Delete(T entity);
    }
}

