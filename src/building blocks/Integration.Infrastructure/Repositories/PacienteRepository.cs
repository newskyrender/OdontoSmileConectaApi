using Integration.Domain.Entities;
using Integration.Domain.Enums;
using Integration.Domain.Repositories;
using Integration.Infrastructure.Contexts;
using Integration.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Integration.Infrastructure.Repositories
{
    public class PacienteRepository : GenericRepository<Paciente>, IPacienteRepository
    {
        private readonly OdontoSmileDataContext _context;

        public PacienteRepository(OdontoSmileDataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Paciente> GetByCpfAsync(string cpf)
        {
            return await _context.Set<Paciente>()
                .FirstOrDefaultAsync(x => x.Cpf == cpf);
        }

        public async Task<bool> ExisteCpfAsync(string cpf)
        {
            return await _context.Set<Paciente>()
                .AnyAsync(x => x.Cpf == cpf);
        }

        public async Task<IEnumerable<Paciente>> GetPacientesAtivosAsync()
        {
            return await _context.Set<Paciente>()
                .Where(x => x.Status == StatusPaciente.Ativo)
                .ToListAsync();
        }

        public async Task<IEnumerable<Paciente>> GetPacientesPorStatusAsync(StatusPaciente status)
        {
            return await _context.Set<Paciente>()
                .Where(x => x.Status == status)
                .ToListAsync();
        }

        public async Task<Paciente> GetByNumeroCooperadoAsync(string numeroCooperado)
        {
            return await _context.Set<Paciente>()
                .FirstOrDefaultAsync(x => x.NumeroCooperado == numeroCooperado);
        }

        public async Task<IEnumerable<Paciente>> GetByNomeAsync(string nome)
        {
            return await _context.Set<Paciente>()
                .Where(x => x.NomeCompleto.ToLower().Contains(nome.ToLower()))
                .OrderBy(x => x.NomeCompleto)
                .ToListAsync();
        }

        public async Task<IEnumerable<Paciente>> GetByCpfOrNomeAsync(string termo)
        {
            return await _context.Set<Paciente>()
                .Where(x => x.Cpf.Contains(termo) || x.NomeCompleto.ToLower().Contains(termo.ToLower()))
                .OrderBy(x => x.NomeCompleto)
                .ToListAsync();
        }
    }
}
