using Integration.Domain.Enums;
using Integration.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Integration.Domain.Http.Request
{
    // Agendamento Requests
    public class AgendamentoRegisterRequest : ICommand
    {
        public Guid? PacienteId { get; set; }
        public Guid ProfissionalId { get; set; }
        public string PacienteNome { get; set; }
        public DateTime DataAgendamento { get; set; }
        
        /// <summary>
        /// Horário de início no formato "HH:mm" (ex: "09:00", "14:30") ou "HH:mm:ss" (ex: "09:00:00")
        /// </summary>
        public string HorarioInicio { get; set; }
        
        public int DuracaoMinutos { get; set; } = 60;
        public ServicoAgendamento Servico { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Observacoes { get; set; }

        /// <summary>
        /// Converte o horário string para TimeSpan
        /// </summary>
        public TimeSpan GetHorarioInicio()
        {
            if (TimeSpan.TryParse(HorarioInicio, out var timeSpan))
                return timeSpan;
            
            throw new ArgumentException($"Formato de horário inválido: {HorarioInicio}. Use formato HH:mm ou HH:mm:ss");
        }
    }
}
