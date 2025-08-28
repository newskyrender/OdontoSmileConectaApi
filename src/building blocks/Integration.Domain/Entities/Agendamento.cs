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
    public class Agendamento : Entity
    {
        protected Agendamento() { }

        public Agendamento(Guid id, Guid profissionalId, string pacienteNome, DateTime dataAgendamento,
            TimeSpan horarioInicio, ServicoAgendamento servico)
        {
            if (id != Guid.Empty) Id = id;
            ProfissionalId = profissionalId;
            PacienteNome = pacienteNome;
            DataAgendamento = dataAgendamento;
            HorarioInicio = horarioInicio;
            Servico = servico;
            Status = StatusAgendamento.Agendado;
            DuracaoMinutos = 60; // padrão

            new ValidationContract<Agendamento>(this)
                .IsRequired(x => x.PacienteNome, "O nome do paciente deve ser informado")
                .IsGreaterThan(x => x.DuracaoMinutos, 0, "A duração deve ser maior que zero");
        }

        public Guid? PacienteId { get; private set; }
        public Guid ProfissionalId { get; private set; }
        public string PacienteNome { get; private set; }
        public DateTime DataAgendamento { get; private set; }
        public TimeSpan HorarioInicio { get; private set; }
        public int DuracaoMinutos { get; private set; }
        public ServicoAgendamento Servico { get; private set; }
        public StatusAgendamento Status { get; private set; }
        public string Telefone { get; private set; }
        public string Email { get; private set; }
        public string Observacoes { get; private set; }
        public string ObservacoesCancelamento { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;

        public virtual Paciente Paciente { get; private set; }
        public virtual Profissional Profissional { get; private set; }

        public void AtualizarStatus(StatusAgendamento novoStatus, string observacoesCancelamento = null)
        {
            Status = novoStatus;
            if (!string.IsNullOrEmpty(observacoesCancelamento))
                ObservacoesCancelamento = observacoesCancelamento;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Reagendar(DateTime novaData, TimeSpan novoHorario)
        {
            DataAgendamento = novaData;
            HorarioInicio = novoHorario;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
