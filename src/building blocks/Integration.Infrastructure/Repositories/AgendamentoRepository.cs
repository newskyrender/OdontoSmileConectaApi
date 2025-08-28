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
    public class AgendamentoRepository : GenericRepository<Agendamento>, IAgendamentoRepository
    {
        private readonly OdontoSmileDataContext _context;

        public AgendamentoRepository(OdontoSmileDataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Agendamento>> GetPorDataAsync(DateTime data)
        {
            return await _context.Set<Agendamento>()
                .Include(x => x.Paciente)
                .Include(x => x.Profissional)
                .Where(x => x.DataAgendamento.Date == data.Date)
                .OrderBy(x => x.HorarioInicio)
                .ToListAsync();
        }

        public async Task<IEnumerable<Agendamento>> GetPorProfissionalAsync(Guid profissionalId)
        {
            return await _context.Set<Agendamento>()
                .Include(x => x.Paciente)
                .Where(x => x.ProfissionalId == profissionalId)
            .OrderBy(x => x.DataAgendamento)
                .ThenBy(x => x.HorarioInicio)
                .ToListAsync();
        }

        public async Task<IEnumerable<Agendamento>> GetPorPacienteAsync(Guid pacienteId)
        {
            return await _context.Set<Agendamento>()
                .Include(x => x.Profissional)
                .Where(x => x.PacienteId == pacienteId)
            .OrderBy(x => x.DataAgendamento)
                .ThenBy(x => x.HorarioInicio)
                .ToListAsync();
        }

        public async Task<IEnumerable<Agendamento>> GetPorStatusAsync(StatusAgendamento status)
        {
            return await _context.Set<Agendamento>()
                .Include(x => x.Paciente)
                .Include(x => x.Profissional)
                .Where(x => x.Status == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<Agendamento>> GetPorPeriodoAsync(DateTime dataInicio, DateTime dataFim)
        {
            return await _context.Set<Agendamento>()
                .Include(x => x.Paciente)
                .Include(x => x.Profissional)
                .Where(x => x.DataAgendamento >= dataInicio && x.DataAgendamento <= dataFim)
                .OrderBy(x => x.DataAgendamento)
                .ThenBy(x => x.HorarioInicio)
                .ToListAsync();
        }

        public async Task<IEnumerable<Agendamento>> GetAgendamentosHojeAsync()
        {
            var hoje = DateTime.Today;
            return await _context.Set<Agendamento>()
                .Include(x => x.Paciente)
                .Include(x => x.Profissional)
                .Where(x => x.DataAgendamento.Date == hoje)
                .OrderBy(x => x.HorarioInicio)
                .ToListAsync();
        }

        public async Task<IEnumerable<Agendamento>> GetPorProfissionalEDataAsync(Guid profissionalId, DateTime data)
        {
            return await _context.Set<Agendamento>()
                .Include(x => x.Paciente)
                .Where(x => x.ProfissionalId == profissionalId && x.DataAgendamento.Date == data.Date)
                .OrderBy(x => x.HorarioInicio)
                .ToListAsync();
        }

        public async Task<bool> VerificarDisponibilidadeAsync(Guid profissionalId, DateTime data, TimeSpan horario, int duracao)
        {
            var conflitos = await GetConflitosAsync(profissionalId, data, horario, duracao);
            return !conflitos.Any();
        }

        public async Task<IEnumerable<Agendamento>> GetConflitosAsync(Guid profissionalId, DateTime data, TimeSpan horario, int duracao)
        {
            var horarioFim = horario.Add(TimeSpan.FromMinutes(duracao));

            return await _context.Set<Agendamento>()
                .Where(x => x.ProfissionalId == profissionalId
                    && x.DataAgendamento.Date == data.Date
                    && x.Status != StatusAgendamento.Cancelado
                    && ((x.HorarioInicio < horarioFim)
                        && (x.HorarioInicio.Add(TimeSpan.FromMinutes(x.DuracaoMinutos)) > horario)))
                .ToListAsync();
        }
    }
}
