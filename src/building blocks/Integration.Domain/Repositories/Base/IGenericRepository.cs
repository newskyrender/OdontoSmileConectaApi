using Integration.Domain.Common;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Integration.Domain.Repositories.Base
{
    public interface IGenericRepository<T> where T : Entity
    {
        Task<T> GetDataAsync(Expression<Func<T, bool>> expression);
        Task<IEnumerable<T>> GetListDataAsync();
        Task<IEnumerable<T>> GetListDataAsync(Expression<Func<T, bool>> expression);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        void Delete(T entity);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> expression);
        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<T, bool>> expression);
        Task<T> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetPagedAsync(int pageNumber, int pageSize);
        Task<IEnumerable<T>> GetPagedAsync(Expression<Func<T, bool>> expression, int pageNumber, int pageSize);
    }
}

