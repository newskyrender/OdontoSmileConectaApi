using Integration.Domain.Common;

namespace Integration.Domain.Http.Request
{
    public class UsuarioUpdateRequest : UsuarioRegisterRequest, ICommand
    {
        public Guid Id { get; set; }
    }
}
