using Integration.Domain.Entities;
using Integration.Domain.Enums;
using Integration.Domain.Repositories.Base;

namespace Integration.Domain.Repositories
{
    public interface ISolicitacaoOrcamentoRepository : IGenericRepository<SolicitacaoOrcamento>
    {
        Task<SolicitacaoOrcamento> GetByNumeroPedidoAsync(string numeroPedido);
        Task<bool> ExisteNumeroPedidoAsync(string numeroPedido);
        Task<IEnumerable<SolicitacaoOrcamento>> GetPorStatusAsync(StatusSolicitacao status);
        Task<IEnumerable<SolicitacaoOrcamento>> GetPorPacienteAsync(Guid pacienteId);
        Task<IEnumerable<SolicitacaoOrcamento>> GetPorProfissionalAsync(Guid profissionalId);
        Task<IEnumerable<SolicitacaoOrcamento>> GetPorTipoTratamentoAsync(TipoTratamento tipoTratamento);
        Task<IEnumerable<SolicitacaoOrcamento>> GetPorPeriodoAsync(DateTime dataInicio, DateTime dataFim);
        Task<string> GerarProximoNumeroPedidoAsync();
    }
}
