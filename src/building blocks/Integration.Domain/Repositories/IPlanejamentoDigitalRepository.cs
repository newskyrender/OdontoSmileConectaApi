using Integration.Domain.Entities;
using Integration.Domain.Enums;
using Integration.Domain.Repositories.Base;

namespace Integration.Domain.Repositories
{
    public interface IPlanejamentoDigitalRepository : IGenericRepository<PlanejamentoDigital>
    {
        Task<IEnumerable<PlanejamentoDigital>> GetPorStatusAsync(StatusPlanejamento status);
        Task<IEnumerable<PlanejamentoDigital>> GetPorPacienteAsync(Guid pacienteId);
        Task<IEnumerable<PlanejamentoDigital>> GetPorProfissionalAsync(Guid profissionalId);
        Task<IEnumerable<PlanejamentoDigital>> GetPorTipoAparelhoAsync(TipoAparelho tipoAparelho);
        Task<IEnumerable<PlanejamentoDigital>> GetPorPrioridadeAsync(PrioridadeCaso prioridade);
        Task<PlanejamentoDigital> GetComSolicitacaoAsync(Guid id);
    }
}
