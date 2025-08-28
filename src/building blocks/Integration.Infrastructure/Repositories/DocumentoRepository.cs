using Integration.Domain.Entities;
using Integration.Domain.Enums;
using Integration.Domain.Repositories;
using Integration.Infrastructure.Contexts;
using Integration.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.Infrastructure.Repositories
{
    public class DocumentoRepository : GenericRepository<Documento>, IDocumentoRepository
    {
        private readonly OdontoSmileDataContext _context;

        public DocumentoRepository(OdontoSmileDataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Documento>> GetPorEntidadeAsync(EntidadeTipo entidadeTipo, Guid entidadeId)
        {
            return await _context.Set<Documento>()
                .Where(x => x.EntidadeTipo == entidadeTipo && x.EntidadeId == entidadeId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Documento>> GetPorTipoAsync(TipoDocumento tipoDocumento)
        {
            return await _context.Set<Documento>()
                .Where(x => x.TipoDocumento == tipoDocumento)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Documento>> GetPorStatusAsync(StatusDocumento status)
        {
            return await _context.Set<Documento>()
            .Where(x => x.Status == status)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<Documento> GetPorCaminhoAsync(string caminhoArquivo)
        {
            return await _context.Set<Documento>()
                .FirstOrDefaultAsync(x => x.CaminhoArquivo == caminhoArquivo);
        }
    }
}
