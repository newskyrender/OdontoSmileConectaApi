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
    public class ProfissionalFacilidadeRepository : GenericRepository<ProfissionalFacilidade>, IProfissionalFacilidadeRepository
    {
        private readonly OdontoSmileDataContext _context;

        public ProfissionalFacilidadeRepository(OdontoSmileDataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProfissionalFacilidade>> GetPorProfissionalAsync(Guid profissionalId)
        {
            return await _context.Set<ProfissionalFacilidade>()
                .Where(x => x.ProfissionalId == profissionalId)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProfissionalFacilidade>> GetPorFacilidadeAsync(Facilidade facilidade)
        {
            return await _context.Set<ProfissionalFacilidade>()
                .Include(x => x.Profissional)
                .Where(x => x.Facilidade == facilidade)
                .ToListAsync();
        }

        public async Task RemovePorProfissionalAsync(Guid profissionalId)
        {
            var facilidades = await GetPorProfissionalAsync(profissionalId);
            foreach (var facilidade in facilidades)
            {
                Delete(facilidade);
            }
        }
    }
}
