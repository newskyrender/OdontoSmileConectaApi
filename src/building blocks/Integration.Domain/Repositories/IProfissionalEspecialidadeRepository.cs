using Integration.Domain.Entities;
using Integration.Domain.Enums;
using Integration.Domain.Repositories.Base;

namespace Integration.Domain.Repositories
{
    public interface IProfissionalEspecialidadeRepository : IGenericRepository<ProfissionalEspecialidade>
    {
        Task<IEnumerable<ProfissionalEspecialidade>> GetPorProfissionalAsync(Guid profissionalId);
        Task<IEnumerable<ProfissionalEspecialidade>> GetPorEspecialidadeAsync(Especialidade especialidade);
        Task RemovePorProfissionalAsync(Guid profissionalId);
    }
}
