using Integration.Domain.Enums;
using Integration.Domain.Common;

namespace Integration.Domain.Http.Request
{
    // Agendamento Requests
    public class AgendamentoRegisterRequest : ICommand
    {
        public Guid? PacienteId { get; set; }
        public Guid ProfissionalId { get; set; }
        public string PacienteNome { get; set; }
        public DateTime DataAgendamento { get; set; }
        public TimeSpan HorarioInicio { get; set; }
        public int DuracaoMinutos { get; set; } = 60;
        public ServicoAgendamento Servico { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Observacoes { get; set; }
    }
}
