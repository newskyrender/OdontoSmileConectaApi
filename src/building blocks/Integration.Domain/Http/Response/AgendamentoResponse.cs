using Integration.Domain.Common;
using Integration.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.Domain.Http.Response
{
    // Agendamento Responses
    public class AgendamentoResponse : ICommandResult
    {
        public Guid Id { get; set; }
        public Guid? PacienteId { get; set; }
        public Guid ProfissionalId { get; set; }
        public string PacienteNome { get; set; }
        public string ProfissionalNome { get; set; }
        public DateTime DataAgendamento { get; set; }
        public TimeSpan HorarioInicio { get; set; }
        public int DuracaoMinutos { get; set; }
        public ServicoAgendamento Servico { get; set; }
        public StatusAgendamento Status { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Observacoes { get; set; }
        public string ObservacoesCancelamento { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
