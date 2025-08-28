using Integration.Domain.Common;
using Integration.Domain.Repositories.Base;
using Integration.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Integration.Infrastructure.Repositories.Base
{
    public class GenericRepository<T> : IGenericRepository<T> where T : Entity
    {
        protected readonly OdontoSmileDataContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(OdontoSmileDataContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<T> GetDataAsync(Expression<Func<T, bool>> expression)
        {
            try
            {
                return await _dbSet.AsNoTracking()
                    .Where(expression)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar dados: {ex.Message}", ex);
            }
        }

        public virtual async Task<IEnumerable<T>> GetListDataAsync()
        {
            try
            {
                return await _dbSet.AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar lista de dados: {ex.Message}", ex);
            }
        }

        public virtual async Task<IEnumerable<T>> GetListDataAsync(Expression<Func<T, bool>> expression)
        {
            try
            {
                return await _dbSet.AsNoTracking()
                    .Where(expression)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar lista de dados com filtro: {ex.Message}", ex);
            }
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                await _dbSet.AddAsync(entity);
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao adicionar entidade: {ex.Message}", ex);
            }
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                var existingEntity = await _dbSet.FindAsync(entity.Id);
                if (existingEntity != null)
                {
                    _context.Entry(existingEntity).CurrentValues.SetValues(entity);
                    _context.Entry(existingEntity).State = EntityState.Modified;
                    return existingEntity;
                }

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao atualizar entidade: {ex.Message}", ex);
            }
        }

        public virtual void Delete(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                if (_context.Entry(entity).State == EntityState.Detached)
                {
                    _dbSet.Attach(entity);
                }

                _dbSet.Remove(entity);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao deletar entidade: {ex.Message}", ex);
            }
        }

        public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> expression)
        {
            try
            {
                return await _dbSet.AnyAsync(expression);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao verificar existência: {ex.Message}", ex);
            }
        }

        public virtual async Task<int> CountAsync()
        {
            try
            {
                return await _dbSet.CountAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao contar registros: {ex.Message}", ex);
            }
        }

        public virtual async Task<int> CountAsync(Expression<Func<T, bool>> expression)
        {
            try
            {
                return await _dbSet.CountAsync(expression);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao contar registros com filtro: {ex.Message}", ex);
            }
        }

        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            try
            {
                return await _dbSet.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar por ID: {ex.Message}", ex);
            }
        }

        public virtual async Task<IEnumerable<T>> GetPagedAsync(int pageNumber, int pageSize)
        {
            try
            {
                if (pageNumber < 1) pageNumber = 1;
                if (pageSize < 1) pageSize = 10;

                return await _dbSet.AsNoTracking()
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar dados paginados: {ex.Message}", ex);
            }
        }

        public virtual async Task<IEnumerable<T>> GetPagedAsync(Expression<Func<T, bool>> expression, int pageNumber, int pageSize)
        {
            try
            {
                if (pageNumber < 1) pageNumber = 1;
                if (pageSize < 1) pageSize = 10;

                return await _dbSet.AsNoTracking()
                    .Where(expression)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar dados paginados com filtro: {ex.Message}", ex);
            }
        }

        protected virtual IQueryable<T> GetQueryable()
        {
            return _dbSet.AsQueryable();
        }

        protected virtual IQueryable<T> GetQueryableAsNoTracking()
        {
            return _dbSet.AsNoTracking();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }

}

