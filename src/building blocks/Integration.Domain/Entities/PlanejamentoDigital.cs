using FluentValidator;
using Integration.Domain.Common;
using Integration.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.Domain.Entities
{
    public class PlanejamentoDigital : Entity
    {
        protected PlanejamentoDigital() { }

        public PlanejamentoDigital(Guid id, Guid pacienteId, Guid profissionalId, int numeroAlinhadores,
            int duracaoTratamentoMeses, decimal orcamentoEstimado, TipoAparelho tipoAparelho)
        {
            if (id != Guid.Empty) Id = id;
            PacienteId = pacienteId;
            ProfissionalId = profissionalId;
            NumeroAlinhadores = numeroAlinhadores;
            DuracaoTratamentoMeses = duracaoTratamentoMeses;
            OrcamentoEstimado = orcamentoEstimado;
            TipoAparelho = tipoAparelho;
            Status = StatusPlanejamento.Rascunho;
            PrioridadeCaso = PrioridadeCaso.Normal;

            new ValidationContract<PlanejamentoDigital>(this)
                .IsGreaterThan(x => x.NumeroAlinhadores, 0, "O número de alinhadores deve ser maior que zero")
                .IsGreaterThan(x => x.DuracaoTratamentoMeses, 0, "A duração do tratamento deve ser maior que zero")
                .IsGreaterThan(x => x.OrcamentoEstimado, 0, "O orçamento estimado deve ser maior que zero");
        }

        public Guid? SolicitacaoOrcamentoId { get; private set; }
        public Guid PacienteId { get; private set; }
        public Guid ProfissionalId { get; private set; }
        public int NumeroAlinhadores { get; private set; }
        public int DuracaoTratamentoMeses { get; private set; }
        public string Observacoes { get; private set; }
        public decimal OrcamentoEstimado { get; private set; }
        public TipoAparelho TipoAparelho { get; private set; }
        public PrioridadeCaso PrioridadeCaso { get; private set; }
        public StatusPlanejamento Status { get; private set; }
        public int? DiasEntreTrocas { get; private set; }
        public int? ConsultasAcompanhamento { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;

        public virtual SolicitacaoOrcamento SolicitacaoOrcamento { get; private set; }
        public virtual Paciente Paciente { get; private set; }
        public virtual Profissional Profissional { get; private set; }

        public void AtualizarStatus(StatusPlanejamento novoStatus)
        {
            Status = novoStatus;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AtualizarObservacoes(string observacoes)
        {
            Observacoes = observacoes;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
