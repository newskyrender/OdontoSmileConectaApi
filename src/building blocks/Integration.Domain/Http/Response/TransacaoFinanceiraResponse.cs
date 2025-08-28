using Integration.Domain.Common;
using Integration.Domain.Enums;

namespace Integration.Domain.Http.Response
{
    public class TransacaoFinanceiraResponse : ICommandResult
    {
        public Guid Id { get; set; }
        public TipoTransacao Tipo { get; set; }
        public decimal Valor { get; set; }
        public string Descricao { get; set; }
        public StatusTransacao Status { get; set; }
        public DateTime? DataVencimento { get; set; }
        public DateTime? DataPagamento { get; set; }
        public string PacienteNome { get; set; }
        public string NumeroCooperado { get; set; }
    }
}
