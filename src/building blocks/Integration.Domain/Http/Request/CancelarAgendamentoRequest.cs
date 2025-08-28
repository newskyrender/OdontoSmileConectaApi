using Integration.Domain.Common;

namespace Integration.Domain.Http.Request
{
    public class CancelarAgendamentoRequest : ICommand
    {
        public Guid Id { get; set; }
        public string ObservacoesCancelamento { get; set; }
    }
}
