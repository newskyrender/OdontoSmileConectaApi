using Integration.Domain.Common;

namespace Integration.Domain.Http.Request
{
    public class PlanejamentoDigitalUpdateRequest : PlanejamentoDigitalRegisterRequest, ICommand
    {
        public Guid Id { get; set; }
    }
}
