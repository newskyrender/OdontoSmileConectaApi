using Integration.Domain.Common;
using Integration.Domain.Enums;

namespace Integration.Domain.Http.Response
{
    // Documento Responses
    public class DocumentoResponse : ICommandResult
    {
        public Guid Id { get; set; }
        public EntidadeTipo EntidadeTipo { get; set; }
        public Guid EntidadeId { get; set; }
        public TipoDocumento TipoDocumento { get; set; }
        public string NomeOriginal { get; set; }
        public string NomeArquivo { get; set; }
        public string CaminhoArquivo { get; set; }
        public long TamanhoBytes { get; set; }
        public string TipoMime { get; set; }
        public StatusDocumento Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
