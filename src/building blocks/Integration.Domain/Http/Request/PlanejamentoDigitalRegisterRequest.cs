using Integration.Domain.Enums;
using Integration.Domain.Common;

namespace Integration.Domain.Http.Request
{
    // PlanejamentoDigital Requests
    public class PlanejamentoDigitalRegisterRequest : ICommand
    {
        public Guid? SolicitacaoOrcamentoId { get; set; }
        public Guid PacienteId { get; set; }
        public Guid ProfissionalId { get; set; }
        public int NumeroAlinhadores { get; set; }
        public int DuracaoTratamentoMeses { get; set; }
        public string Observacoes { get; set; }
        public decimal OrcamentoEstimado { get; set; }
        public TipoAparelho TipoAparelho { get; set; }
        public PrioridadeCaso PrioridadeCaso { get; set; }
        public int? DiasEntreTrocas { get; set; }
        public int? ConsultasAcompanhamento { get; set; }
    }
}
