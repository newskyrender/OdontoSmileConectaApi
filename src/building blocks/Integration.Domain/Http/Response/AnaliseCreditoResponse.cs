using Integration.Domain.Common;
using Integration.Domain.Enums;

namespace Integration.Domain.Http.Response
{
    public class AnaliseCreditoResponse : ICommandResult
    {
        public Guid Id { get; set; }
        public int? ScoreCredito { get; set; }
        public decimal? RendaDeclarada { get; set; }
        public decimal? ValorSolicitado { get; set; }
        public decimal? ValorAprovado { get; set; }
        public ResultadoAnaliseCredito Resultado { get; set; }
        public string MotivoRejeicao { get; set; }
        public TipoAnalise TipoAnalise { get; set; }
        public DateTime DataAnalise { get; set; }
        public string PacienteNome { get; set; }
    }
}
