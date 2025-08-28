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
    // Repositories para entidades relacionais
    public class ProfissionalEspecialidadeRepository : GenericRepository<ProfissionalEspecialidade>, IProfissionalEspecialidadeRepository
    {
        private readonly OdontoSmileDataContext _context;

        public ProfissionalEspecialidadeRepository(OdontoSmileDataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProfissionalEspecialidade>> GetPorProfissionalAsync(Guid profissionalId)
        {
            return await _context.Set<ProfissionalEspecialidade>()
            .Where(x => x.ProfissionalId == profissionalId)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProfissionalEspecialidade>> GetPorEspecialidadeAsync(Especialidade especialidade)
        {
            return await _context.Set<ProfissionalEspecialidade>()
                .Include(x => x.Profissional)
                .Where(x => x.Especialidade == especialidade)
                .ToListAsync();
        }

        public async Task RemovePorProfissionalAsync(Guid profissionalId)
        {
            var especialidades = await GetPorProfissionalAsync(profissionalId);
            foreach (var especialidade in especialidades)
            {
                Delete(especialidade);
            }
        }
    }
}
