using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.Infrastructure.Transactions
{
    public class TransactionManager : ITransactionManager
    {
        private readonly IUow _uow;
        private bool _disposed = false;

        public TransactionManager(IUow uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        public async Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> operation)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));

            var transaction = await _uow.BeginTransactionAsync();

            try
            {
                var result = await operation();
                await _uow.CommitAsync();
                return result;
            }
            catch (Exception)
            {
                await _uow.RollbackAsync();
                throw;
            }
        }

        public async Task ExecuteInTransactionAsync(Func<Task> operation)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));

            var transaction = await _uow.BeginTransactionAsync();

            try
            {
                await operation();
                await _uow.CommitAsync();
            }
            catch (Exception)
            {
                await _uow.RollbackAsync();
                throw;
            }
        }

        public T ExecuteInTransaction<T>(Func<T> operation)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));

            var transaction = _uow.BeginTransaction();

            try
            {
                var result = operation();
                _uow.Commit();
                return result;
            }
            catch (Exception)
            {
                _uow.Rollback();
                throw;
            }
        }

        public void ExecuteInTransaction(Action operation)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));

            var transaction = _uow.BeginTransaction();

            try
            {
                operation();
                _uow.Commit();
            }
            catch (Exception)
            {
                _uow.Rollback();
                throw;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                // Verifique se IUow possui um método Dispose. Se não, remova esta linha ou implemente IDisposable na interface IUow.
                if (_uow is IDisposable disposableUow)
                {
                    disposableUow.Dispose();
                }
                _disposed = true; 
            }
        }

        ~TransactionManager()
        {
            Dispose(false);
        }
    }
}

