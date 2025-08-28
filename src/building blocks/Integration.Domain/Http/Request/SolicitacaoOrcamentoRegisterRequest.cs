using Integration.Domain.Enums;
using Integration.Domain.Common;

namespace Integration.Domain.Http.Request
{
    // SolicitacaoOrcamento Requests
    public class SolicitacaoOrcamentoRegisterRequest : ICommand
    {
        public string NomeCompleto { get; set; }
        public string Cpf { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public TipoTratamento TipoTratamento { get; set; }
        public string Observacoes { get; set; }
        public Guid? ProfissionalId { get; set; }
    }
}
