using Integration.Domain.Common;

namespace Integration.Domain.Http.Request
{
    public class ProfissionalUpdateRequest : ProfissionalRegisterRequest, ICommand
    {
        public Guid Id { get; set; }
    }
}
