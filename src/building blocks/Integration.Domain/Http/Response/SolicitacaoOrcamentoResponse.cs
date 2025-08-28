using Integration.Domain.Common;
using Integration.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.Domain.Http.Response
{
    // SolicitacaoOrcamento Responses
    public class SolicitacaoOrcamentoResponse : ICommandResult
    {
        public Guid Id { get; set; }
        public string NumeroPedido { get; set; }
        public Guid? PacienteId { get; set; }
        public string NomeCompleto { get; set; }
        public string Cpf { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public TipoTratamento TipoTratamento { get; set; }
        public string Observacoes { get; set; }
        public StatusSolicitacao Status { get; set; }
        public decimal? ValorAprovado { get; set; }
        public int? NumeroParcelas { get; set; }
        public decimal? ValorParcela { get; set; }
        public Guid? ProfissionalId { get; set; }
        public string ProfissionalNome { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
