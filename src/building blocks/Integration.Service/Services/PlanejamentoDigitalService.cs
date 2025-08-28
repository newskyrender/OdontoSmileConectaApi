using AutoMapper;
using FluentValidator;
using Integration.Domain.Common;
using Integration.Domain.Entities;
using Integration.Domain.Enums;
using Integration.Domain.Http.Request;
using Integration.Domain.Http.Response;
using Integration.Domain.Repositories;
using Integration.Infrastructure.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.Service.Services
{
    public class PlanejamentoDigitalService : Notifiable
    {
        private readonly IPlanejamentoDigitalRepository _repository;
        private readonly IPacienteRepository _pacienteRepository;
        private readonly IProfissionalRepository _profissionalRepository;
        private readonly IMapper _mapper;
        private readonly IUow _uow;

        public PlanejamentoDigitalService(IPlanejamentoDigitalRepository repository,
            IPacienteRepository pacienteRepository, IProfissionalRepository profissionalRepository,
            IMapper mapper, IUow uow)
        {
            _repository = repository;
            _pacienteRepository = pacienteRepository;
            _profissionalRepository = profissionalRepository;
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<ICommandResult> Handle(Guid id)
        {
            var entity = await _repository.GetComSolicitacaoAsync(id);

            if (entity is null) AddNotification("Alert", "Planejamento não encontrado");

            if (!IsValid()) return default;

            return _mapper.Map<PlanejamentoDigitalResponse>(entity);
        }

        public async Task<IEnumerable<ICommandResult>> Listar()
        {
            var entities = await _repository.GetListDataAsync();

            if (!entities.Any()) AddNotification("Alert", "Nenhum planejamento encontrado");

            if (!IsValid()) return default;

            return _mapper.Map<List<PlanejamentoDigitalResponse>>(entities);
        }

        public async Task<IEnumerable<ICommandResult>> GetPorStatus(StatusPlanejamento status)
        {
            var entities = await _repository.GetPorStatusAsync(status);

            if (!entities.Any()) AddNotification("Alert", "Nenhum planejamento encontrado com este status");

            if (!IsValid()) return default;

            return _mapper.Map<List<PlanejamentoDigitalResponse>>(entities);
        }

        public async Task<ICommandResult> Handle(PlanejamentoDigitalRegisterRequest request)
        {
            // Verificar se paciente e profissional existem
            var paciente = await _pacienteRepository.GetDataAsync(x => x.Id == request.PacienteId);
            if (paciente is null)
                AddNotification("Warning", "Paciente não encontrado");

            var profissional = await _profissionalRepository.GetDataAsync(x => x.Id == request.ProfissionalId);
            if (profissional is null)
                AddNotification("Warning", "Profissional não encontrado");

            if (!IsValid()) return default;

            var entity = new PlanejamentoDigital(default, request.PacienteId, request.ProfissionalId,
                request.NumeroAlinhadores, request.DuracaoTratamentoMeses, request.OrcamentoEstimado,
                request.TipoAparelho);

            AddNotifications(entity.Notifications);

            if (!IsValid()) return default;

            await _repository.AddAsync(entity);
            await _uow.CommitAsync();

            return _mapper.Map<PlanejamentoDigitalResponse>(entity);
        }

        public async Task<ICommandResult> AlterarStatus(Guid id, StatusPlanejamento novoStatus)
        {
            var entity = await _repository.GetDataAsync(x => x.Id == id);

            if (entity is null) AddNotification("Warning", "Planejamento não encontrado");

            if (!IsValid()) return default;

            entity.AtualizarStatus(novoStatus);

            await _repository.UpdateAsync(entity);
            await _uow.CommitAsync();

            return _mapper.Map<PlanejamentoDigitalResponse>(entity);
        }

        public async Task<ICommandResult> Delete(Guid id)
        {
            var entity = await _repository.GetDataAsync(x => x.Id == id);

            if (entity is null) AddNotification("Warning", "Planejamento não encontrado");

            if (!IsValid()) return default;

            _repository.Delete(entity);
            await _uow.CommitAsync();

            return _mapper.Map<PlanejamentoDigitalResponse>(entity);
        }
    }
}
