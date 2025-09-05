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
    public class ProfissionalRepository : GenericRepository<Profissional>, IProfissionalRepository
    {
        private readonly OdontoSmileDataContext _context;

        public ProfissionalRepository(OdontoSmileDataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Profissional> GetByCpfAsync(string cpf)
        {
            return await _context.Set<Profissional>()
                .FirstOrDefaultAsync(x => x.Cpf == cpf);
        }

        public async Task<Profissional> GetByCroAsync(string cro)
        {
            return await _context.Set<Profissional>()
                .FirstOrDefaultAsync(x => x.Cro == cro);
        }

        public async Task<bool> ExisteCpfAsync(string cpf)
        {
            return await _context.Set<Profissional>()
                .AnyAsync(x => x.Cpf == cpf);
        }

        public async Task<bool> ExisteCroAsync(string cro)
        {
            return await _context.Set<Profissional>()
                .AnyAsync(x => x.Cro == cro);
        }

        public async Task<bool> ExisteEmailAsync(string email)
        {
            return await _context.Set<Profissional>()
                .AnyAsync(x => x.EmailProfissional == email);
        }

        public async Task<IEnumerable<Profissional>> GetProfissionaisAtivosAsync()
        {
            return await _context.Set<Profissional>()
                .Where(x => x.Ativo && x.StatusAprovacao == StatusAprovacao.Aprovado)
                .ToListAsync();
        }

        public async Task<IEnumerable<Profissional>> GetPorStatusAprovacaoAsync(StatusAprovacao status)
        {
            return await _context.Set<Profissional>()
                .Where(x => x.StatusAprovacao == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<Profissional>> GetPorEspecialidadeAsync(Especialidade especialidade)
        {
            return await _context.Set<Profissional>()
                .Where(x => x.Especialidades.Any(e => e.Especialidade == especialidade))
                .ToListAsync();
        }

        public async Task<Profissional> GetComEspecialidadesAsync(Guid id)
        {
            return await _context.Set<Profissional>()
                .Include(x => x.Especialidades)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Profissional> GetComEquipamentosAsync(Guid id)
        {
            return await _context.Set<Profissional>()
                .Include(x => x.Equipamentos)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Profissional> GetComFacilidadesAsync(Guid id)
        {
            return await _context.Set<Profissional>()
                .Include(x => x.Facilidades)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Profissional> GetCompletoAsync(Guid id)
        {
            return await _context.Set<Profissional>()
                .Include(x => x.Especialidades)
                .Include(x => x.Equipamentos)
                .Include(x => x.Facilidades)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Profissional>> GetByNomeAsync(string nome)
        {
            return await _context.Set<Profissional>()
                .Where(x => x.NomeCompleto.Contains(nome))
                .ToListAsync();
        }
    }
}
