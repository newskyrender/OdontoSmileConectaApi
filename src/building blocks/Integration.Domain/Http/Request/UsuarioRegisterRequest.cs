using Integration.Domain.Common;

namespace Integration.Domain.Http.Request
{
    // Usuario Requests
    public class UsuarioRegisterRequest : ICommand
    {
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Nome { get; set; }
        public bool Ativo { get; set; } = true;
    }

}
