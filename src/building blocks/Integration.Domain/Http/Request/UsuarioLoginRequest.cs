using Integration.Domain.Common;

namespace Integration.Domain.Http.Request
{
    public class UsuarioLoginRequest : ICommand
    {
        public string Email { get; set; }
        public string Senha { get; set; }
    }
}
