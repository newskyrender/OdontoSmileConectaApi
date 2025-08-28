using Integration.Domain.Entities;
using Integration.Domain.Enums;
using Integration.Domain.Repositories.Base;

namespace Integration.Domain.Repositories
{
    public interface IProfissionalEquipamentoRepository : IGenericRepository<ProfissionalEquipamento>
    {
        Task<IEnumerable<ProfissionalEquipamento>> GetPorProfissionalAsync(Guid profissionalId);
        Task<IEnumerable<ProfissionalEquipamento>> GetPorEquipamentoAsync(Equipamento equipamento);
        Task RemovePorProfissionalAsync(Guid profissionalId);
    }
}
