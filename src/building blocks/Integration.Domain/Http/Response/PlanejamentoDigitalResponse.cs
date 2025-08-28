using Integration.Domain.Common;
using Integration.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.Domain.Http.Response
{
    // PlanejamentoDigital Responses
    public class PlanejamentoDigitalResponse : ICommandResult
    {
        public Guid Id { get; set; }
        public Guid? SolicitacaoOrcamentoId { get; set; }
        public Guid PacienteId { get; set; }
        public string PacienteNome { get; set; }
        public Guid ProfissionalId { get; set; }
        public string ProfissionalNome { get; set; }
        public int NumeroAlinhadores { get; set; }
        public int DuracaoTratamentoMeses { get; set; }
        public string Observacoes { get; set; }
        public decimal OrcamentoEstimado { get; set; }
        public TipoAparelho TipoAparelho { get; set; }
        public PrioridadeCaso PrioridadeCaso { get; set; }
        public StatusPlanejamento Status { get; set; }
        public int? DiasEntreTrocas { get; set; }
        public int? ConsultasAcompanhamento { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
