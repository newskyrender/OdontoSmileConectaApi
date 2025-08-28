using Integration.Domain.Entities;
using Integration.Domain.Enums;
using Integration.Domain.Repositories.Base;

namespace Integration.Domain.Repositories
{
    public interface IDocumentoRepository : IGenericRepository<Documento>
    {
        Task<IEnumerable<Documento>> GetPorEntidadeAsync(EntidadeTipo entidadeTipo, Guid entidadeId);
        Task<IEnumerable<Documento>> GetPorTipoAsync(TipoDocumento tipoDocumento);
        Task<IEnumerable<Documento>> GetPorStatusAsync(StatusDocumento status);
        Task<Documento> GetPorCaminhoAsync(string caminhoArquivo);
    }
}
