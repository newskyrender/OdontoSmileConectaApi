using Integration.Domain.Entities;
using Integration.Domain.Repositories;
using Integration.Infrastructure.Contexts;
using Integration.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Integration.Infrastructure.Repositories
{
    public class UsuarioRepository : GenericRepository<Usuario>, IUsuarioRepository
    {
        private readonly OdontoSmileDataContext _context;

        public UsuarioRepository(OdontoSmileDataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Usuario> GetByEmailAsync(string email)
        {
            return await _context.Set<Usuario>()
                .FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<bool> ExisteEmailAsync(string email)
        {
            return await _context.Set<Usuario>()
                .AnyAsync(x => x.Email == email);
        }
    }
}
