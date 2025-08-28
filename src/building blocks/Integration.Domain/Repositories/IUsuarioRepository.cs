using Integration.Domain.Entities;
using Integration.Domain.Repositories.Base;

namespace Integration.Domain.Repositories
{
    public interface IUsuarioRepository : IGenericRepository<Usuario>
    {
        Task<Usuario> GetByEmailAsync(string email);
        Task<bool> ExisteEmailAsync(string email);
    }
}
