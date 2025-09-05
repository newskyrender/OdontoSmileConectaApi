using Integration.Domain.Common;

namespace Integration.Domain.Http.Request
{
    public class AlterarSenhaRequest : ICommand
    {
        public Guid Id { get; set; }
        public string SenhaAtual { get; set; }
        public string NovaSenha { get; set; }
    }
}
