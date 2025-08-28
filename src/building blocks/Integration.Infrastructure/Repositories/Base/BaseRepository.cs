using Integration.Domain.Common;
using Integration.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Integration.Infrastructure.Repositories.Base
{
    // Repository base específico para casos que precisam de operações mais complexas
    public abstract class BaseRepository<T> : GenericRepository<T> where T : Entity
    {
        protected BaseRepository(OdontoSmileDataContext context) : base(context) { }

        protected virtual async Task<IEnumerable<T>> GetWithIncludesAsync(params Expression<Func<T, object>>[] includes)
        {
            try
            {
                IQueryable<T> query = _dbSet.AsNoTracking();

                foreach (var include in includes)
                {
                    query = query.Include(include);
                }

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar dados com includes: {ex.Message}", ex);
            }
        }

        protected virtual async Task<T> GetWithIncludesAsync(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes)
        {
            try
            {
                IQueryable<T> query = _dbSet.AsNoTracking();

                foreach (var include in includes)
                {
                    query = query.Include(include);
                }

                return await query.FirstOrDefaultAsync(expression);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar dados com includes e filtro: {ex.Message}", ex);
            }
        }

        protected virtual async Task<IEnumerable<T>> GetOrderedAsync<TKey>(Expression<Func<T, TKey>> orderBy, bool ascending = true)
        {
            try
            {
                IQueryable<T> query = _dbSet.AsNoTracking();

                query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar dados ordenados: {ex.Message}", ex);
            }
        }

        protected virtual async Task<IEnumerable<T>> GetFilteredAndOrderedAsync<TKey>(
            Expression<Func<T, bool>> filter,
            Expression<Func<T, TKey>> orderBy,
            bool ascending = true)
        {
            try
            {
                IQueryable<T> query = _dbSet.AsNoTracking().Where(filter);

                query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar dados filtrados e ordenados: {ex.Message}", ex);
            }
        }

        protected virtual async Task<IEnumerable<T>> GetPagedAndOrderedAsync<TKey>(
            Expression<Func<T, TKey>> orderBy,
            int pageNumber,
            int pageSize,
            bool ascending = true)
        {
            try
            {
                if (pageNumber < 1) pageNumber = 1;
                if (pageSize < 1) pageSize = 10;

                IQueryable<T> query = _dbSet.AsNoTracking();

                query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);

                return await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar dados paginados e ordenados: {ex.Message}", ex);
            }
        }

        protected virtual async Task<IEnumerable<T>> GetComplexQueryAsync(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            int? skip = null,
            int? take = null,
            params Expression<Func<T, object>>[] includes)
        {
            try
            {
                IQueryable<T> query = _dbSet.AsNoTracking();

                if (filter != null)
                    query = query.Where(filter);

                foreach (var include in includes)
                {
                    query = query.Include(include);
                }

                if (orderBy != null)
                    query = orderBy(query);

                if (skip.HasValue)
                    query = query.Skip(skip.Value);

                if (take.HasValue)
                    query = query.Take(take.Value);

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao executar consulta complexa: {ex.Message}", ex);
            }
        }

        protected virtual async Task<bool> BulkUpdateAsync(Expression<Func<T, bool>> filter, Expression<Func<T, T>> updateExpression)
        {
            try
            {
                var entities = await _dbSet.Where(filter).ToListAsync();

                foreach (var entity in entities)
                {
                    _context.Entry(entity).State = EntityState.Modified;
                }

                return entities.Any();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro na atualização em lote: {ex.Message}", ex);
            }
        }

        protected virtual async Task<int> BulkDeleteAsync(Expression<Func<T, bool>> filter)
        {
            try
            {
                var entities = await _dbSet.Where(filter).ToListAsync();

                foreach (var entity in entities)
                {
                    _dbSet.Remove(entity);
                }

                return entities.Count;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro na exclusão em lote: {ex.Message}", ex);
            }
        }

        protected virtual async Task<IEnumerable<TResult>> GetProjectedAsync<TResult>(Expression<Func<T, TResult>> projection)
        {
            try
            {
                return await _dbSet.AsNoTracking()
                    .Select(projection)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar dados projetados: {ex.Message}", ex);
            }
        }

        protected virtual async Task<IEnumerable<TResult>> GetProjectedAsync<TResult>(
            Expression<Func<T, bool>> filter,
            Expression<Func<T, TResult>> projection)
        {
            try
            {
                return await _dbSet.AsNoTracking()
                    .Where(filter)
                    .Select(projection)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar dados projetados com filtro: {ex.Message}", ex);
            }
        }
    }
}
