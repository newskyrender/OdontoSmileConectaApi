using Integration.Domain.Common;
using Integration.Domain.Enums;

namespace Integration.Domain.Http.Request
{
    public class DocumentoFileUploadRequest : ICommand
    {
        public EntidadeTipo EntidadeTipo { get; set; }
        public Guid EntidadeId { get; set; }
        public TipoDocumento TipoDocumento { get; set; }
        // Não precisamos dos outros campos já que eles serão preenchidos pelo serviço
    }
}
