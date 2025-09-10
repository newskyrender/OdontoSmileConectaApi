using Integration.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Integration.Domain.Http.Request
{
    public class ReagendarRequest : ICommand
    {
        public Guid Id { get; set; }
        public DateTime NovaDataAgendamento { get; set; }
        
        /// <summary>
        /// Novo horário de início no formato "HH:mm" (ex: "09:00", "14:30") ou "HH:mm:ss" (ex: "09:00:00")
        /// </summary>
        public string NovoHorarioInicio { get; set; }

        /// <summary>
        /// Converte o horário string para TimeSpan
        /// </summary>
        public TimeSpan GetNovoHorarioInicio()
        {
            if (TimeSpan.TryParse(NovoHorarioInicio, out var timeSpan))
                return timeSpan;
            
            throw new ArgumentException($"Formato de horário inválido: {NovoHorarioInicio}. Use formato HH:mm ou HH:mm:ss");
        }
    }
}
