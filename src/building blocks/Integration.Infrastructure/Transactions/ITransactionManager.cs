using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.Infrastructure.Transactions
{
    // Interface para transações mais complexas
    public interface ITransactionManager : IDisposable
    {
        Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> operation);
        Task ExecuteInTransactionAsync(Func<Task> operation);
        T ExecuteInTransaction<T>(Func<T> operation);
        void ExecuteInTransaction(Action operation);
    }
}
