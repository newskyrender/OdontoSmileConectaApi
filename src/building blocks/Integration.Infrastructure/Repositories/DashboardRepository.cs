using Microsoft.EntityFrameworkCore;
using Integration.Domain.Repositories;
using Integration.Domain.Http.Response;
using Integration.Domain.Enums;
using Integration.Infrastructure.Contexts;

namespace Integration.Infrastructure.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly OdontoSmileDataContext _context;

        public DashboardRepository(OdontoSmileDataContext context)
        {
            _context = context;
        }

        public async Task<DashboardMetricasResponse> GetMetricasAsync()
        {
            var hoje = DateTime.Today;
            var inicioMes = new DateTime(hoje.Year, hoje.Month, 1);

            var totalPacientes = await GetTotalPacientesAsync();
            var pacientesAtivos = await GetPacientesAtivosAsync();
            var agendamentosHoje = await GetAgendamentosHojeAsync();
            var confirmadosHoje = await GetConfirmadosHojeAsync();
            var totalSolicitacoes = await GetTotalSolicitacoesAsync();
            var totalAprovadas = await GetTotalAprovadasAsync();
            var volumeTotal = await GetVolumeTotalAsync();
            var taxaAprovacao = await GetTaxaAprovacaoAsync();

            return new DashboardMetricasResponse
            {
                TotalPacientes = totalPacientes,
                PacientesAtivos = pacientesAtivos,
                AgendamentosHoje = agendamentosHoje,
                ConfirmadosHoje = confirmadosHoje,
                TotalSolicitacoes = totalSolicitacoes,
                TotalAprovadas = totalAprovadas,
                VolumeTotal = volumeTotal,
                TaxaAprovacao = taxaAprovacao
            };
        }

        public async Task<int> GetTotalPacientesAsync()
        {
            return await _context.Pacientes.CountAsync();
        }

        public async Task<int> GetPacientesAtivosAsync()
        {
            return await _context.Pacientes
                .Where(x => x.Status == StatusPaciente.Ativo)
                .CountAsync();
        }

        public async Task<int> GetAgendamentosHojeAsync()
        {
            var hoje = DateTime.Today;
            return await _context.Agendamentos
                .Where(x => x.DataAgendamento.Date == hoje)
                .CountAsync();
        }

        public async Task<int> GetConfirmadosHojeAsync()
        {
            var hoje = DateTime.Today;
            return await _context.Agendamentos
                .Where(x => x.DataAgendamento.Date == hoje && x.Status == StatusAgendamento.Confirmado)
                .CountAsync();
        }

        public async Task<int> GetTotalSolicitacoesAsync()
        {
            var inicioMes = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            return await _context.SolicitacoesOrcamento
                .Where(x => x.CreatedAt >= inicioMes)
                .CountAsync();
        }

        public async Task<int> GetTotalAprovadasAsync()
        {
            var inicioMes = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            return await _context.SolicitacoesOrcamento
                .Where(x => x.CreatedAt >= inicioMes && x.Status == StatusSolicitacao.Aprovado)
                .CountAsync();
        }

        public async Task<decimal> GetVolumeTotalAsync()
        {
            var inicioMes = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            return await _context.SolicitacoesOrcamento
                .Where(x => x.CreatedAt >= inicioMes && x.Status == StatusSolicitacao.Aprovado && x.ValorAprovado.HasValue)
                .SumAsync(x => x.ValorAprovado.Value);
        }

        public async Task<decimal> GetTaxaAprovacaoAsync()
        {
            var inicioMes = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

            var total = await _context.SolicitacoesOrcamento
                .Where(x => x.CreatedAt >= inicioMes)
                .CountAsync();

            if (total == 0) return 0;

            var aprovadas = await _context.SolicitacoesOrcamento
                .Where(x => x.CreatedAt >= inicioMes && x.Status == StatusSolicitacao.Aprovado)
                .CountAsync();

            return Math.Round((decimal)aprovadas / total * 100, 2);
        }
    }
}
