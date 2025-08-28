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
    public class ProfissionalEquipamentoRepository : GenericRepository<ProfissionalEquipamento>, IProfissionalEquipamentoRepository
    {
        private readonly OdontoSmileDataContext _context;

        public ProfissionalEquipamentoRepository(OdontoSmileDataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProfissionalEquipamento>> GetPorProfissionalAsync(Guid profissionalId)
        {
            return await _context.Set<ProfissionalEquipamento>()
                .Where(x => x.ProfissionalId == profissionalId)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProfissionalEquipamento>> GetPorEquipamentoAsync(Equipamento equipamento)
        {
            return await _context.Set<ProfissionalEquipamento>()
                .Include(x => x.Profissional)
                .Where(x => x.Equipamento == equipamento)
                .ToListAsync();
        }

        public async Task RemovePorProfissionalAsync(Guid profissionalId)
        {
            var equipamentos = await GetPorProfissionalAsync(profissionalId);
            foreach (var equipamento in equipamentos)
            {
                Delete(equipamento);
            }
        }
    }
}
