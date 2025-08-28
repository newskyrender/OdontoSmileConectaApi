using Integration.Domain.Common;

namespace Integration.Domain.Http.Response
{
    // Usuario Responses
    public class UsuarioResponse : ICommandResult
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Nome { get; set; }
        public bool Ativo { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
