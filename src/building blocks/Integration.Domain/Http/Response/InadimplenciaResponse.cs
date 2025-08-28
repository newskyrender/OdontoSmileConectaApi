using Integration.Domain.Common;
using Integration.Domain.Enums;

namespace Integration.Domain.Http.Response
{
    public class InadimplenciaResponse : ICommandResult
    {
        public Guid Id { get; set; }
        public decimal ValorDevido { get; set; }
        public int DiasAtraso { get; set; }
        public StatusInadimplencia Status { get; set; }
        public int ContatosRealizados { get; set; }
        public DateTime? DataUltimoContato { get; set; }
        public decimal? AcordoValor { get; set; }
        public DateTime? AcordoData { get; set; }
        public DateTime DataInicioAtraso { get; set; }
        public string PacienteNome { get; set; }
        public string PacienteTelefone { get; set; }
    }
}
