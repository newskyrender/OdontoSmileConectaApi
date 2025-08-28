using Integration.Domain.Common;

namespace Integration.Domain.Http.Request
{
    public class SolicitacaoOrcamentoUpdateRequest : SolicitacaoOrcamentoRegisterRequest, ICommand
    {
        public Guid Id { get; set; }
    }
}
