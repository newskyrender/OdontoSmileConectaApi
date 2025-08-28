using Integration.Domain.Common;

namespace Integration.Domain.Http.Request
{
    public class PacienteUpdateRequest : PacienteRegisterRequest, ICommand
    {
        public Guid Id { get; set; }
    }
}
