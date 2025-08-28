using AutoMapper;
using FluentValidator;
using Integration.Domain.Common;
using Integration.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.Service.Services
{
    public class RelatorioFinanceiroService : Notifiable
    {
        private readonly IRelatorioFinanceiroRepository _repository;
        private readonly IMapper _mapper;

        public RelatorioFinanceiroService(IRelatorioFinanceiroRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ICommandResult> GetRelatorioCompleto(DateTime? dataInicio = null, DateTime? dataFim = null)
        {
            var relatorio = await _repository.GetRelatorioCompletoAsync(dataInicio, dataFim);

            if (relatorio is null) AddNotification("Alert", "Erro ao gerar relatório financeiro");

            if (!IsValid()) return default;

            return relatorio;
        }

        public async Task<IEnumerable<ICommandResult>> GetTransacoes(DateTime? dataInicio = null, DateTime? dataFim = null)
        {
            var transacoes = await _repository.GetTransacoesAsync(dataInicio, dataFim);

            if (!transacoes.Any()) AddNotification("Alert", "Nenhuma transação encontrada");

            if (!IsValid()) return default;

            return transacoes;
        }

        public async Task<IEnumerable<ICommandResult>> GetInadimplencias()
        {
            var inadimplencias = await _repository.GetInadimplenciasAsync();

            if (!inadimplencias.Any()) AddNotification("Info", "Nenhuma inadimplência encontrada");

            if (!IsValid()) return default;

            return inadimplencias;
        }
    }
}
