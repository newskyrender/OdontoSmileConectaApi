using Integration.Domain.Http.Response;

namespace Integration.Domain.Repositories
{
    // Interfaces para relatórios e dashboard
    public interface IDashboardRepository
    {
        Task<DashboardMetricasResponse> GetMetricasAsync();
        Task<int> GetTotalPacientesAsync();
        Task<int> GetPacientesAtivosAsync();
        Task<int> GetAgendamentosHojeAsync();
        Task<int> GetConfirmadosHojeAsync();
        Task<int> GetTotalSolicitacoesAsync();
        Task<int> GetTotalAprovadasAsync();
        Task<decimal> GetVolumeTotalAsync();
        Task<decimal> GetTaxaAprovacaoAsync();
    }
}
