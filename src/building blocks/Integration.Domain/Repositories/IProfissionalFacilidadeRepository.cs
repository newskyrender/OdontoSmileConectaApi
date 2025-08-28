using Integration.Domain.Entities;
using Integration.Domain.Enums;
using Integration.Domain.Repositories.Base;

namespace Integration.Domain.Repositories
{
    public interface IProfissionalFacilidadeRepository : IGenericRepository<ProfissionalFacilidade>
    {
        Task<IEnumerable<ProfissionalFacilidade>> GetPorProfissionalAsync(Guid profissionalId);
        Task<IEnumerable<ProfissionalFacilidade>> GetPorFacilidadeAsync(Facilidade facilidade);
        Task RemovePorProfissionalAsync(Guid profissionalId);
    }
}
