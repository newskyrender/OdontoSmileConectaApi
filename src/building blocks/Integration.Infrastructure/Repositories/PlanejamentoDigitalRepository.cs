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
    public class PlanejamentoDigitalRepository : GenericRepository<PlanejamentoDigital>, IPlanejamentoDigitalRepository
    {
        private readonly OdontoSmileDataContext _context;

        public PlanejamentoDigitalRepository(OdontoSmileDataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PlanejamentoDigital>> GetPorStatusAsync(StatusPlanejamento status)
        {
            return await _context.Set<PlanejamentoDigital>()
                .Include(x => x.Paciente)
            .Include(x => x.Profissional)
                .Where(x => x.Status == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<PlanejamentoDigital>> GetPorPacienteAsync(Guid pacienteId)
        {
            return await _context.Set<PlanejamentoDigital>()
                .Include(x => x.Profissional)
                .Where(x => x.PacienteId == pacienteId)
                .ToListAsync();
        }

        public async Task<IEnumerable<PlanejamentoDigital>> GetPorProfissionalAsync(Guid profissionalId)
        {
            return await _context.Set<PlanejamentoDigital>()
                .Include(x => x.Paciente)
                .Where(x => x.ProfissionalId == profissionalId)
                .ToListAsync();
        }

        public async Task<IEnumerable<PlanejamentoDigital>> GetPorTipoAparelhoAsync(TipoAparelho tipoAparelho)
        {
            return await _context.Set<PlanejamentoDigital>()
                .Include(x => x.Paciente)
                .Include(x => x.Profissional)
                .Where(x => x.TipoAparelho == tipoAparelho)
                .ToListAsync();
        }

        public async Task<IEnumerable<PlanejamentoDigital>> GetPorPrioridadeAsync(PrioridadeCaso prioridade)
        {
            return await _context.Set<PlanejamentoDigital>()
                .Include(x => x.Paciente)
            .Include(x => x.Profissional)
                .Where(x => x.PrioridadeCaso == prioridade)
                .ToListAsync();
        }

        public async Task<PlanejamentoDigital> GetComSolicitacaoAsync(Guid id)
        {
            return await _context.Set<PlanejamentoDigital>()
                .Include(x => x.SolicitacaoOrcamento)
                .Include(x => x.Paciente)
                .Include(x => x.Profissional)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
