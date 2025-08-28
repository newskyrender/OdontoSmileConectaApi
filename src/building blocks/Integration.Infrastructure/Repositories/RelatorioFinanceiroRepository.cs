using Microsoft.EntityFrameworkCore;
using Integration.Domain.Repositories;
using Integration.Domain.Http.Response;
using Integration.Domain.Enums;
using Integration.Infrastructure.Contexts;

namespace Integration.Infrastructure.Repositories
{
    public class RelatorioFinanceiroRepository : IRelatorioFinanceiroRepository
    {
        private readonly OdontoSmileDataContext _context;

        public RelatorioFinanceiroRepository(OdontoSmileDataContext context)
        {
            _context = context;
        }

        public async Task<RelatorioFinanceiroResponse> GetRelatorioCompletoAsync(DateTime? dataInicio = null, DateTime? dataFim = null)
        {
            var transacoes = await GetTransacoesAsync(dataInicio, dataFim);
            var analisesCredito = await GetAnalisesCredito​Async(dataInicio, dataFim);
            var inadimplencias = await GetInadimplenciasAsync();

            var volumeTotal = await GetVolumeFinanceiroAsync(dataInicio, dataFim);
            var taxaAprovacao = await GetTaxaAprovacaoCreditoAsync(dataInicio, dataFim);

            // Calcular valores recebidos e pendentes (simulado - seria implementado com dados reais)
            var totalRecebido = transacoes.Where(x => x.Status == StatusTransacao.Aprovado).Sum(x => x.Valor);
            var totalPendente = transacoes.Where(x => x.Status == StatusTransacao.Pendente).Sum(x => x.Valor);

            return new RelatorioFinanceiroResponse
            {
                Transacoes = transacoes.ToList(),
                AnalisesCredito = analisesCredito.ToList(),
                Inadimplencias = inadimplencias.ToList(),
                TotalVolume = volumeTotal,
                TotalRecebido = totalRecebido,
                TotalPendente = totalPendente,
                TaxaAprovacao = taxaAprovacao
            };
        }

        public async Task<IEnumerable<TransacaoFinanceiraResponse>> GetTransacoesAsync(DateTime? dataInicio = null, DateTime? dataFim = null)
        {
            // Como não temos tabela de transações financeiras no script, vamos simular com base nas solicitações aprovadas
            var query = _context.SolicitacoesOrcamento
                .Include(x => x.Paciente)
                .Where(x => x.Status == StatusSolicitacao.Aprovado && x.ValorAprovado.HasValue);

            if (dataInicio.HasValue)
                query = query.Where(x => x.CreatedAt >= dataInicio.Value);

            if (dataFim.HasValue)
                query = query.Where(x => x.CreatedAt <= dataFim.Value);

            var solicitacoes = await query.ToListAsync();

            return solicitacoes.Select(x => new TransacaoFinanceiraResponse
            {
                Id = x.Id,
                Tipo = TipoTransacao.AprovacaoCredito,
                Valor = x.ValorAprovado.Value,
                Descricao = $"Aprovação de crédito - {x.TipoTratamento}",
                Status = StatusTransacao.Aprovado,
                DataVencimento = DateTime.Today.AddDays(30), // Simulado
                DataPagamento = null,
                PacienteNome = x.Paciente?.NomeCompleto ?? x.NomeCompleto,
                NumeroCooperado = x.Paciente?.NumeroCooperado ?? ""
            });
        }

        public async Task<IEnumerable<AnaliseCreditoResponse>> GetAnalisesCredito​Async(DateTime? dataInicio = null, DateTime? dataFim = null)
        {
            // Simulação baseada nas solicitações de orçamento
            var query = _context.SolicitacoesOrcamento
                .Include(x => x.Paciente)
                .AsQueryable();

            if (dataInicio.HasValue)
                query = query.Where(x => x.CreatedAt >= dataInicio.Value);

            if (dataFim.HasValue)
                query = query.Where(x => x.CreatedAt <= dataFim.Value);

            var solicitacoes = await query.ToListAsync();

            return solicitacoes.Select(x => new AnaliseCreditoResponse
            {
                Id = x.Id,
                ScoreCredito = GenerateRandomScore(), // Simulado
                RendaDeclarada = x.Paciente?.RendaMensal,
                ValorSolicitado = x.ValorAprovado ?? 0,
                ValorAprovado = x.ValorAprovado,
                Resultado = x.Status == StatusSolicitacao.Aprovado ? ResultadoAnaliseCredito.AprovadoAutomatico :
                           x.Status == StatusSolicitacao.Rejeitado ? ResultadoAnaliseCredito.Rejeitado :
                           ResultadoAnaliseCredito.PendenteDocumentos,
                MotivoRejeicao = x.Status == StatusSolicitacao.Rejeitado ? "Score de crédito insuficiente" : null,
                TipoAnalise = TipoAnalise.Automatica,
                DataAnalise = x.UpdatedAt,
                PacienteNome = x.Paciente?.NomeCompleto ?? x.NomeCompleto
            });
        }

        public async Task<IEnumerable<InadimplenciaResponse>> GetInadimplenciasAsync()
        {
            // Simulação de inadimplências
            var solicitacoesVencidas = await _context.SolicitacoesOrcamento
                .Include(x => x.Paciente)
                .Where(x => x.Status == StatusSolicitacao.Aprovado && x.ValorAprovado.HasValue)
                .ToListAsync();

            // Simular algumas inadimplências (20% das aprovadas)
            var inadimplentes = solicitacoesVencidas.Take(solicitacoesVencidas.Count / 5);

            return inadimplentes.Select(x => new InadimplenciaResponse
            {
                Id = x.Id,
                ValorDevido = x.ValorParcela ?? x.ValorAprovado.Value,
                DiasAtraso = GenerateRandomDays(),
                Status = StatusInadimplencia.Ativo,
                ContatosRealizados = GenerateRandomContacts(),
                DataUltimoContato = DateTime.Today.AddDays(-GenerateRandomDays()),
                AcordoValor = null,
                AcordoData = null,
                DataInicioAtraso = DateTime.Today.AddDays(-GenerateRandomDays()),
                PacienteNome = x.Paciente?.NomeCompleto ?? x.NomeCompleto,
                PacienteTelefone = x.Paciente?.CelularPrincipal ?? x.Telefone
            });
        }

        public async Task<decimal> GetVolumeFinanceiroAsync(DateTime? dataInicio = null, DateTime? dataFim = null)
        {
            var query = _context.SolicitacoesOrcamento
                .Where(x => x.Status == StatusSolicitacao.Aprovado && x.ValorAprovado.HasValue);

            if (dataInicio.HasValue)
                query = query.Where(x => x.CreatedAt >= dataInicio.Value);

            if (dataFim.HasValue)
                query = query.Where(x => x.CreatedAt <= dataFim.Value);

            return await query.SumAsync(x => x.ValorAprovado.Value);
        }

        public async Task<decimal> GetTaxaAprovacaoCreditoAsync(DateTime? dataInicio = null, DateTime? dataFim = null)
        {
            var query = _context.SolicitacoesOrcamento.AsQueryable();

            if (dataInicio.HasValue)
                query = query.Where(x => x.CreatedAt >= dataInicio.Value);

            if (dataFim.HasValue)
                query = query.Where(x => x.CreatedAt <= dataFim.Value);

            var total = await query.CountAsync();
            if (total == 0) return 0;

            var aprovadas = await query.Where(x => x.Status == StatusSolicitacao.Aprovado).CountAsync();

            return Math.Round((decimal)aprovadas / total * 100, 2);
        }

        // Métodos auxiliares para simulação
        private int GenerateRandomScore()
        {
            var random = new Random();
            return random.Next(300, 850);
        }

        private int GenerateRandomDays()
        {
            var random = new Random();
            return random.Next(1, 90);
        }

        private int GenerateRandomContacts()
        {
            var random = new Random();
            return random.Next(0, 10);
        }
    }
}
