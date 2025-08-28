using Integration.Domain.Common;

namespace Integration.Domain.Http.Response
{
    // Relatórios Response
    public class RelatorioFinanceiroResponse : ICommandResult
    {
        public List<TransacaoFinanceiraResponse> Transacoes { get; set; } = new List<TransacaoFinanceiraResponse>();
        public List<AnaliseCreditoResponse> AnalisesCredito { get; set; } = new List<AnaliseCreditoResponse>();
        public List<InadimplenciaResponse> Inadimplencias { get; set; } = new List<InadimplenciaResponse>();
        public decimal TotalVolume { get; set; }
        public decimal TotalRecebido { get; set; }
        public decimal TotalPendente { get; set; }
        public decimal TaxaAprovacao { get; set; }
    }
}
