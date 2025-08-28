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
    public class SolicitacaoOrcamentoRepository : GenericRepository<SolicitacaoOrcamento>, ISolicitacaoOrcamentoRepository
    {
        private readonly OdontoSmileDataContext _context;

        public SolicitacaoOrcamentoRepository(OdontoSmileDataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<SolicitacaoOrcamento> GetByNumeroPedidoAsync(string numeroPedido)
        {
            return await _context.Set<SolicitacaoOrcamento>()
                .Include(x => x.Paciente)
                .Include(x => x.Profissional)
                .FirstOrDefaultAsync(x => x.NumeroPedido == numeroPedido);
        }

        public async Task<bool> ExisteNumeroPedidoAsync(string numeroPedido)
        {
            return await _context.Set<SolicitacaoOrcamento>()
                .AnyAsync(x => x.NumeroPedido == numeroPedido);
        }

        public async Task<IEnumerable<SolicitacaoOrcamento>> GetPorStatusAsync(StatusSolicitacao status)
        {
            return await _context.Set<SolicitacaoOrcamento>()
                .Include(x => x.Paciente)
            .Include(x => x.Profissional)
                .Where(x => x.Status == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<SolicitacaoOrcamento>> GetPorPacienteAsync(Guid pacienteId)
        {
            return await _context.Set<SolicitacaoOrcamento>()
                .Include(x => x.Profissional)
                .Where(x => x.PacienteId == pacienteId)
                .ToListAsync();
        }

        public async Task<IEnumerable<SolicitacaoOrcamento>> GetPorProfissionalAsync(Guid profissionalId)
        {
            return await _context.Set<SolicitacaoOrcamento>()
                .Include(x => x.Paciente)
            .Where(x => x.ProfissionalId == profissionalId)
                .ToListAsync();
        }

        public async Task<IEnumerable<SolicitacaoOrcamento>> GetPorTipoTratamentoAsync(TipoTratamento tipoTratamento)
        {
            return await _context.Set<SolicitacaoOrcamento>()
                .Include(x => x.Paciente)
                .Include(x => x.Profissional)
            .Where(x => x.TipoTratamento == tipoTratamento)
                .ToListAsync();
        }

        public async Task<IEnumerable<SolicitacaoOrcamento>> GetPorPeriodoAsync(DateTime dataInicio, DateTime dataFim)
        {
            return await _context.Set<SolicitacaoOrcamento>()
                .Include(x => x.Paciente)
                .Include(x => x.Profissional)
                .Where(x => x.CreatedAt >= dataInicio && x.CreatedAt <= dataFim)
                .ToListAsync();
        }

        public async Task<string> GerarProximoNumeroPedidoAsync()
        {
            var ultimoNumero = await _context.Set<SolicitacaoOrcamento>()
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => x.NumeroPedido)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(ultimoNumero))
                return $"PED{DateTime.Now:yyyyMMdd}0001";

            // Extrair número sequencial e incrementar
            var prefixo = $"PED{DateTime.Now:yyyyMMdd}";
            var ultimosHoje = await _context.Set<SolicitacaoOrcamento>()
                .Where(x => x.NumeroPedido.StartsWith(prefixo))
                .CountAsync();

            return $"{prefixo}{(ultimosHoje + 1):0000}";
        }
    }
}
