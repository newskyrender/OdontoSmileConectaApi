using Integration.Domain.Common;

namespace Integration.Domain.Http.Request
{
    public class AgendamentoUpdateRequest : AgendamentoRegisterRequest, ICommand
    {
        public Guid Id { get; set; }
    }
}
