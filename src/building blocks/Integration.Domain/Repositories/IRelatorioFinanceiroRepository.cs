using Integration.Domain.Http.Response;

namespace Integration.Domain.Repositories
{
    public interface IRelatorioFinanceiroRepository
    {
        Task<RelatorioFinanceiroResponse> GetRelatorioCompletoAsync(DateTime? dataInicio = null, DateTime? dataFim = null);
        Task<IEnumerable<TransacaoFinanceiraResponse>> GetTransacoesAsync(DateTime? dataInicio = null, DateTime? dataFim = null);
        Task<IEnumerable<AnaliseCreditoResponse>> GetAnalisesCreditoAsync(DateTime? dataInicio = null, DateTime? dataFim = null);
        Task<IEnumerable<InadimplenciaResponse>> GetInadimplenciasAsync();
        Task<decimal> GetVolumeFinanceiroAsync(DateTime? dataInicio = null, DateTime? dataFim = null);
        Task<decimal> GetTaxaAprovacaoCreditoAsync(DateTime? dataInicio = null, DateTime? dataFim = null);
    }
}
