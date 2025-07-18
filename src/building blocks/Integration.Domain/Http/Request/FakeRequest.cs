using Integration.Domain.Common;

namespace Integration.Domain.Http.Request
{
    public class FakeRegisterRequest : ICommand
    {
        public string Nome { get; set; }
        public string Email { get; set; }
    }

    public class FakeUpdateRequest : FakeRegisterRequest, ICommand
    {
        public Guid Id { get; set; }
    }
}

