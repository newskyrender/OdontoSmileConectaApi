using Microsoft.EntityFrameworkCore.Storage;

namespace Integration.Infrastructure.Transactions
{
    public interface IUow
    {
        Task CommitAsync();
        Task RollbackAsync();
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task<int> SaveChangesAsync();
        void Commit();
        void Rollback();
        IDbContextTransaction BeginTransaction();
        int SaveChanges();
    }
}

