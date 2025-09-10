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
        public AgendamentoRepository(OdontoSmileDataContext context) : base(context)
        {
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

            // Buscar todos os agendamentos do profissional no dia (exceto cancelados)
            var agendamentos = await _context.Set<Agendamento>()
                .Where(x => x.ProfissionalId == profissionalId
                    && x.DataAgendamento.Date == data.Date
                    && x.Status != StatusAgendamento.Cancelado)
                .ToListAsync();

            // Verificar conflitos em memória para evitar problemas de tradução SQL
            var conflitos = agendamentos.Where(x =>
            {
                var agendamentoFim = x.HorarioInicio.Add(TimeSpan.FromMinutes(x.DuracaoMinutos));
                return (x.HorarioInicio < horarioFim) && (agendamentoFim > horario);
            });

            return conflitos;
        }

        public async Task<int> GetConsultasHojeCountAsync()
        {
            var hoje = DateTime.Today;
            return await _context.Set<Agendamento>()
                .CountAsync(x => x.DataAgendamento.Date == hoje);
        }

        public async Task<IEnumerable<Agendamento>> GetConsultasHojeAsync()
        {
            var hoje = DateTime.Today;
            return await _context.Set<Agendamento>()
                .Include(x => x.Paciente)
                .Include(x => x.Profissional)
                .Where(x => x.DataAgendamento.Date == hoje)
                .OrderBy(x => x.HorarioInicio)
                .ToListAsync();
        }

        public async Task<IEnumerable<Agendamento>> GetConsultasPendentesAsync()
        {
            return await _context.Set<Agendamento>()
                .Include(x => x.Paciente)
                .Include(x => x.Profissional)
                .Where(x => x.Status == StatusAgendamento.Agendado && x.DataAgendamento >= DateTime.Today)
                .OrderBy(x => x.DataAgendamento)
                .ThenBy(x => x.HorarioInicio)
                .ToListAsync();
        }

        public async Task<IEnumerable<Agendamento>> GetCancelamentosSemanaAsync(DateTime inicioSemana, DateTime fimSemana)
        {
            return await _context.Set<Agendamento>()
                .Include(x => x.Paciente)
                .Include(x => x.Profissional)
                .Where(x => x.Status == StatusAgendamento.Cancelado 
                           && x.DataAgendamento.Date >= inicioSemana.Date 
                           && x.DataAgendamento.Date <= fimSemana.Date)
                .ToListAsync();
        }

        public async Task<Agendamento> GetUltimaConsultaPacienteAsync(Guid pacienteId)
        {
            return await _context.Set<Agendamento>()
                .Where(x => x.PacienteId == pacienteId && x.DataAgendamento < DateTime.Today)
                .OrderByDescending(x => x.DataAgendamento)
                .ThenByDescending(x => x.HorarioInicio)
                .FirstOrDefaultAsync();
        }

        public async Task<Agendamento> GetProximaConsultaPacienteAsync(Guid pacienteId)
        {
            return await _context.Set<Agendamento>()
                .Where(x => x.PacienteId == pacienteId 
                           && x.DataAgendamento >= DateTime.Today 
                           && x.Status != StatusAgendamento.Cancelado)
                .OrderBy(x => x.DataAgendamento)
                .ThenBy(x => x.HorarioInicio)
                .FirstOrDefaultAsync();
        }
    }
}
