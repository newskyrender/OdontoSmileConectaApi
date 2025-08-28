using Integration.Infrastructure.Contexts;

namespace Integration.Infrastructure.Transactions
{
    public class Uow : IUow
    {
        private readonly IntegrationDataContext _context;

        public Uow(IntegrationDataContext context)
        {
            _context = context;
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Rollback()
        {
            // Do Nothing
        }
    }
}

