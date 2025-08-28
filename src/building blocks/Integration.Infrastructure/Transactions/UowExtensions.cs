using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.Infrastructure.Transactions
{
    // Extensões úteis para o UoW
    public static class UowExtensions
    {
        public static async Task<T> ExecuteAsync<T>(this IUow uow, Func<Task<T>> operation)
        {
            try
            {
                var result = await operation();
                await uow.CommitAsync();
                return result;
            }
            catch (Exception)
            {
                await uow.RollbackAsync();
                throw;
            }
        }

        public static async Task ExecuteAsync(this IUow uow, Func<Task> operation)
        {
            try
            {
                await operation();
                await uow.CommitAsync();
            }
            catch (Exception)
            {
                await uow.RollbackAsync();
                throw;
            }
        }

        public static T Execute<T>(this IUow uow, Func<T> operation)
        {
            try
            {
                var result = operation();
                uow.Commit();
                return result;
            }
            catch (Exception)
            {
                uow.Rollback();
                throw;
            }
        }

        public static void Execute(this IUow uow, Action operation)
        {
            try
            {
                operation();
                uow.Commit();
            }
            catch (Exception)
            {
                uow.Rollback();
                throw;
            }
        }
    }
}
