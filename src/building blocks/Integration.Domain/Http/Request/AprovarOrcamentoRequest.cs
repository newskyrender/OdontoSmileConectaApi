using Integration.Domain.Common;

namespace Integration.Domain.Http.Request
{
    public class AprovarOrcamentoRequest : ICommand
    {
        public Guid Id { get; set; }
        public decimal ValorAprovado { get; set; }
        public int NumeroParcelas { get; set; }
        public decimal ValorParcela { get; set; }
    }
}
