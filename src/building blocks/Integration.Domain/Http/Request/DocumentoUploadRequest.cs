using Integration.Domain.Enums;
using Integration.Domain.Common;

namespace Integration.Domain.Http.Request
{
    // Documento Requests
    public class DocumentoUploadRequest : ICommand
    {
        public EntidadeTipo EntidadeTipo { get; set; }
        public Guid EntidadeId { get; set; }
        public TipoDocumento TipoDocumento { get; set; }
        public string NomeOriginal { get; set; }
        public string NomeArquivo { get; set; }
        public string CaminhoArquivo { get; set; }
        public int TamanhoBytes { get; set; }
        public string TipoMime { get; set; }
    }
}
