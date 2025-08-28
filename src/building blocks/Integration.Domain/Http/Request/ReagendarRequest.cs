using Integration.Domain.Common;

namespace Integration.Domain.Http.Request
{
    public class ReagendarRequest : ICommand
    {
        public Guid Id { get; set; }
        public DateTime NovaDataAgendamento { get; set; }
        public TimeSpan NovoHorarioInicio { get; set; }
    }
}
