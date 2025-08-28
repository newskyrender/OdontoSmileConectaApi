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
    public class DashboardService : Notifiable
    {
        private readonly IDashboardRepository _repository;
        private readonly IMapper _mapper;

        public DashboardService(IDashboardRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ICommandResult> GetMetricas()
        {
            var metricas = await _repository.GetMetricasAsync();

            if (metricas is null) AddNotification("Alert", "Erro ao obter métricas do dashboard");

            if (!IsValid()) return default;

            return metricas;
        }
    }
}
