using Integration.Domain.Common;

namespace Integration.Domain.Http.Response
{
    // Dashboard Responses
    public class DashboardMetricasResponse : ICommandResult
    {
        public int TotalPacientes { get; set; }
        public int PacientesAtivos { get; set; }
        public int AgendamentosHoje { get; set; }
        public int ConfirmadosHoje { get; set; }
        public int TotalSolicitacoes { get; set; }
        public int TotalAprovadas { get; set; }
        public decimal VolumeTotal { get; set; }
        public decimal TaxaAprovacao { get; set; }
    }
}
