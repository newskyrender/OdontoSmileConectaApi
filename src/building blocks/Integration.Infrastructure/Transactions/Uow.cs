using Integration.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore.Storage;

namespace Integration.Infrastructure.Transactions
{
    public class Uow : IUow
    {
        private readonly OdontoSmileDataContext _context;
        private IDbContextTransaction _transaction;
        private bool _disposed = false;

        public Uow(OdontoSmileDataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task CommitAsync()
        {
            try
            {
                await _context.SaveChangesAsync();

                if (_transaction != null)
                {
                    await _transaction.CommitAsync();
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
            catch (Exception ex)
            {
                await RollbackAsync();
                throw new Exception("Erro ao confirmar transa��o", ex);
            }
        }

        public async Task RollbackAsync()
        {
            try
            {
                if (_transaction != null)
                {
                    await _transaction.RollbackAsync();
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao reverter transa��o", ex);
            }
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            try
            {
                if (_transaction != null)
                {
                    throw new InvalidOperationException("Uma transa��o j� est� ativa");
                }

                _transaction = await _context.Database.BeginTransactionAsync();
                return _transaction;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao iniciar transa��o", ex);
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao salvar altera��es", ex);
            }
        }

        // M�todos s�ncronos
        public void Commit()
        {
            try
            {
                _context.SaveChanges();

                if (_transaction != null)
                {
                    _transaction.Commit();
                    _transaction.Dispose();
                    _transaction = null;
                }
            }
            catch (Exception ex)
            {
                Rollback();
                throw new Exception("Erro ao confirmar transa��o", ex);
            }
        }

        public void Rollback()
        {
            try
            {
                if (_transaction != null)
                {
                    _transaction.Rollback();
                    _transaction.Dispose();
                    _transaction = null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao reverter transa��o", ex);
            }
        }

        public IDbContextTransaction BeginTransaction()
        {
            try
            {
                if (_transaction != null)
                {
                    throw new InvalidOperationException("Uma transa��o j� est� ativa");
                }

                _transaction = _context.Database.BeginTransaction();
                return _transaction;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao iniciar transa��o", ex);
            }
        }

        public int SaveChanges()
        {
            try
            {
                return _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao salvar altera��es", ex);
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
                try
                {
                    if (_transaction != null)
                    {
                        _transaction.Dispose();
                        _transaction = null;
                    }

                    _context?.Dispose();
                }
                catch (Exception ex)
                {
                    // Log do erro se necess�rio
                    Console.WriteLine($"Erro ao fazer dispose: {ex.Message}");
                }
                finally
                {
                    _disposed = true;
                }
            }
        }

        ~Uow()
        {
            Dispose(false);
        }
    }
}

