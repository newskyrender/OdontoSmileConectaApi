using Integration.Domain.Common;

namespace Integration.Domain.Http.Response
{
    public class FakeResponse : ICommandResult
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
    }
}

